using FluentValidation;
using System.Threading;
using TimeSeries.Application.Models;
using TimeSeries.Domain.Enums;

namespace TimeSeries.Application.Imports
{
    public class TimeSeriesProcessor
    {
        private readonly IValidator<TimeSeriesDto> _validator;
        public TimeSeriesProcessor(IValidator<TimeSeriesDto> validator)
        {
            _validator = validator;
        }

        public async Task<ProcessedTimeSeriesResult> Process(TimeSeriesData timeSeriesData, CancellationToken cancellationToken)
        {
            var validSeries = new List<TimeSeriesDto>();
            int failedCount = 0;

            foreach (var entry in timeSeriesData.TimeSeries)
            {
                // Validera fälten med TimeSeriesValidator
                var result = await _validator.ValidateAsync(entry, cancellationToken);

                // Konvertera energienhet om annan än den interna (kWh)
                if (timeSeriesData.EnergyUnit == EnergyUnit.MWh)
                    entry.Quantity *= 1000;

                if (result.IsValid)
                    validSeries.Add(entry);
                else
                    failedCount++;
            }

            return new ProcessedTimeSeriesResult(validSeries, failedCount);
        }
    }

}
