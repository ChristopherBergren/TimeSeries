using TimeSeries.Application.Models;
using TimeSeries.Domain.Enums;

namespace TimeSeries.Application.Interfaces
{
    public interface ILoadProfileService
    {
        Task UpsertTimeSeries(List<TimeSeriesDto> timeSeries, MeasurementUnit unit, CancellationToken cancellationToken);
    }
}
