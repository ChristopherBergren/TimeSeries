using FluentValidation;
using TimeSeriesRoot.Application.Models;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.Imports
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

            // Skapa ett gemensamt serie-id (som används i uppgift 2.2)
            var seriesId = Guid.NewGuid();

            foreach (var entry in timeSeriesData.TimeSeries)
            {
                // Validera fälten med TimeSeriesValidator
                var result = await _validator.ValidateAsync(entry, cancellationToken);

                // Konvertera energienhet om annan än den interna (kWh)
                if (timeSeriesData.EnergyUnit == EnergyUnit.MWh)
                    entry.Quantity *= 1000;

                // Nytt grupp-id läggs till alla datapunkter. Om det senare i pipelinen (databas-merge)
                // visar sig att datapunkten är en dubblett och enbart en update skall utföras, kommer inte
                // detta nya värde att ersätta det gamla. Enbart de domän-specifika fälten uppdateras.
                entry.SeriesId = seriesId;

                if (result.IsValid)
                    validSeries.Add(entry);
                else
                    failedCount++;
            }

            return new ProcessedTimeSeriesResult(validSeries, failedCount);
        }
    }

}
