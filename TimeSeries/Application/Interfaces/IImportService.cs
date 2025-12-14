using TimeSeries.Application.Models;
using TimeSeries.Application.Responses;
using TimeSeries.Domain.Enums;

namespace TimeSeries.Application.Interfaces
{
    public interface IImportService
    {
        Task<UpsertTimeSeriesResponse> ExecuteBulkImport(CancellationToken cancellationToken);
        Task<UpsertTimeSeriesResponse> ImportTimeSeries(List<TimeSeriesDto> timeSeries, EnergyUnit unit, CancellationToken cancellationToken);

    }
}
