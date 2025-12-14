using TimeSeriesRoot.Application.Models;
using TimeSeriesRoot.Application.Responses;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.Interfaces
{
    public interface IImportService
    {
        Task<ImportTimeSeriesResponse> BulkImportTimeSeries(CancellationToken cancellationToken);
        Task<ImportTimeSeriesResponse> ImportTimeSeries(List<TimeSeriesDto> timeSeries, EnergyUnit unit, CancellationToken cancellationToken);

    }
}
