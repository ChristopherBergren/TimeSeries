using TimeSeries.Application.Models;
using TimeSeries.Application.Responses;
using TimeSeries.Domain.Enums;

namespace TimeSeries.Application.Interfaces
{
    public interface ILoadProfileService
    {
        Task<UpsertTimeSeriesResponse> UpsertTimeSeries(List<TimeSeriesDto> timeSeries, MeasurementUnit unit, CancellationToken cancellationToken);
    }
}
