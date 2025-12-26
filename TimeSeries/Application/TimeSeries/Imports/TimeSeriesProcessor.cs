using FluentValidation;
using Serilog;
using TimeSeriesRoot.Application.TimeSeries.Interfaces;
using TimeSeriesRoot.Application.TimeSeries.Models;
using TimeSeriesRoot.Domain.Entities;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.TimeSeries.Imports
{
    public class TimeSeriesProcessor
    {
        private readonly IValidator<TimeSeriesDto> _validator;
        private readonly ITimeSeriesRepository _repository;
        public TimeSeriesProcessor(IValidator<TimeSeriesDto> validator, ITimeSeriesRepository repository)
        {
            _validator = validator;
            _repository = repository;
        }

        public async Task<ProcessedTimeSeriesResult> Process(TimeSeriesData timeSeriesData, int seriesId, CancellationToken cancellationToken)
        {
            var validSeries = new List<TimeSeriesDto>();
            int failedCount = 0;

            foreach (var entry in timeSeriesData.TimeSeries)
            {
                // Validera fälten med TimeSeriesValidator
                var result = await _validator.ValidateAsync(entry, cancellationToken);
                if (!result.IsValid)
                {
                    Log.Warning($"Validation failed for: {entry.ToString()}");
                    failedCount++;
                    continue;
                }

                // Konvertera energienhet om annan än den interna (kWh)
                if (timeSeriesData.EnergyUnit == EnergyUnit.MWh)
                {
                    entry.Quantity = Math.Round(entry.GetQuantity() * 1000, 6).ToString("G");
                }

                // Nytt serie-id läggs till alla datapunkter. Om det senare i pipelinen (databas-merge)
                // visar sig att datapunkten är en dubblett och enbart en update skall utföras, kommer inte
                // detta nya värde att ersätta det gamla. Enbart de domän-specifika fälten uppdateras.
                entry.SeriesId = seriesId;

                validSeries.Add(entry);
            }

            return new ProcessedTimeSeriesResult(validSeries, failedCount);
        }
    }

}
