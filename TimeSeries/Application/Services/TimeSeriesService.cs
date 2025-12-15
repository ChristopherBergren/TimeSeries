using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Linq;
using TimeSeriesRoot.Application.Interfaces;
using TimeSeriesRoot.Application.Models;
using TimeSeriesRoot.Application.Responses;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.Services
{
    public class TimeSeriesService : ITimeSeriesService
    {
        private readonly Settings _settings;
        private readonly ITimeSeriesRepository _repository;
        private record EnergyLevel(DateTime Time, double Level);

        public TimeSeriesService(IOptions<Settings> settings, ITimeSeriesRepository repository)
        {
            _settings = settings.Value;
            _repository = repository;
        }

        public async Task<GetTimeSeriesResponse> GetTimeSeries(int page, int pageSize)
        {
            var timeSeriesDto = await _repository.GetTimeSeriesAsync(page, pageSize);
            var response = new GetTimeSeriesResponse { TimeSeries = timeSeriesDto };

            return response;
        }

        public async Task<GetTimeSeriesByIdResponse> GetTimeSeriesById(int seriesId, EnergyUnit unit, string start, string end)
        {
            // Hämta alla datapunkter för angiven serie
            var timeSeriesDto = await _repository.GetTimeSeriesByIdAsync(seriesId);

            // Konvertera om MWh är valt (kWh default i db)
            if (unit == EnergyUnit.MWh)
                timeSeriesDto.ForEach(s => s.Quantity = Math.Round((double)(s.Quantity! / 1000), 6));

            // Filtrera ut posterna inom angivet tidsintervall
            var from = ParseTime(start);
            var to = ParseTime(end);
            timeSeriesDto = timeSeriesDto.Where(d => ((DateTime)d.Timestamp!).TimeOfDay >= from && ((DateTime)d.Timestamp!).TimeOfDay <= to).ToList();

            var response = new GetTimeSeriesByIdResponse { TimeSeries = timeSeriesDto };

            return response;
        }

        public async Task<GetTimeSeriesKpiResponse> GetTimeSeriesKpi(int seriesId, string periodStart, string periodEnd)
        {
            var start = DateTime.Parse(periodStart);
            var end = DateTime.Parse(periodEnd);

            // Hämta alla datapunkter för angiven serie
            var timeSeriesDto = await _repository.GetTimeSeriesInPeriodAsync(seriesId, start, end);
            var levels = timeSeriesDto.Select(s => new EnergyLevel(((DateTime)s.Timestamp!), -(double)s.Quantity! / 1000)).ToList();

            return ComputeKpiValues(levels);
        }

        private GetTimeSeriesKpiResponse ComputeKpiValues(List<EnergyLevel> levels)
        {
            // Innan beräkningarna har jag redan negerat alla mätvärden. Jag tror jag tänker rätt där.

            var intervalCount = levels.Select(l => l.Time).Distinct().Count();
            var avg = levels.Average(l => l.Level);
            var intervalHours = 0.25; // Kvartsdata som jag förstått det, samma som på 50Hertz =)
            var totalEnergy = levels.Sum(l => l.Level); // Total energi i MWh
            var maxLevel = levels.Max(l => l.Level);
            var averageLoad = (totalEnergy / intervalCount) / intervalHours; // Snittbelastning i MW
            var peakLoad = maxLevel / intervalHours; // Max belastning i MW
            var minLoad = levels.Min(l => l.Level) / intervalHours;
            var loadFactor = averageLoad / peakLoad;
            var stdDev = StandardDeviation(levels.Select(l => l.Level).ToList());
            var timeOfMax = levels.First(l => l.Level == peakLoad * intervalHours).Time;
            var timeOfMin = levels.First(l => l.Level == minLoad * intervalHours).Time;

            var response = new GetTimeSeriesKpiResponse
            {
                Kpis = new List<Kpi>
                {
                    new("Total energi (MWh)", totalEnergy),
                    new("Snittbelastning (MW)", averageLoad),
                    new("Maxbelastning (MW)", peakLoad),
                    new($"Tidpunkt för maxbelastning = {timeOfMax}", 0),
                    new("Minbelastning (MW)", minLoad),
                    new($"Tidpunkt för minbelastning = {timeOfMin}", 0),
                    new("Belastningsgrad (Snitt/Max)", loadFactor),
                    new("Standardavvikelse", stdDev),
                }
            };

            return response;
        }

        private static double StandardDeviation(List<double> values)
        {
            var average = values.Average();
            var sumOfSquares = values.Sum(v => Math.Pow(v - average, 2));

            return Math.Sqrt(sumOfSquares / values.Count);
        }
        private TimeSpan ParseTime(string value)
        {
            var parts = value.Split(':');
            var h = int.Parse(parts[0]);
            var m = parts.Length > 1 ? int.Parse(parts[1]) : 0;
            var s = parts.Length > 2 ? int.Parse(parts[2]) : 0;
            return new TimeSpan(h, m, s);
        }
    }
}