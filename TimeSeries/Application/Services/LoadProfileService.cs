using Castle.Core.Resource;
using FluentValidation;
using Serilog;
using TimeSeries.Application.Interfaces;
using TimeSeries.Application.Models;
using TimeSeries.Domain.Enums;
using TimeSeries.Infrastructure;

namespace TimeSeries.Application.Services
{
    public class LoadProfileService(IValidator<TimeSeriesDto> validator, ILoadProfileRepository loadProfileRepository) : ILoadProfileService
    {
        public async Task UpsertTimeSeries(List<TimeSeriesDto> timeSeries, MeasurementUnit unit, CancellationToken cancellationToken)
        {
            // cancellationToken.ThrowIfCancellationRequested();


            var validOrders = new List<TimeSeriesDto>();
            var failedOrders = new List<(TimeSeriesDto Order, string Error)>();

            foreach (var entry in timeSeries)
            {
                var result = await validator.ValidateAsync(entry, cancellationToken);
                if (result.IsValid)
                {
                    validOrders.Add(entry);
                }
                else
                {
                    var errorMessages = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                    failedOrders.Add((entry, errorMessages));
                }
            }

            await loadProfileRepository.UpsertLoadProfileAsync(cancellationToken);
        }
    }
}
