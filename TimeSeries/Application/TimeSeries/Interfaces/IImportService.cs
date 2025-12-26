using TimeSeriesRoot.Application.TimeSeries.Models;
using TimeSeriesRoot.Application.TimeSeries.Responses;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.TimeSeries.Interfaces
{
    public interface IImportService
    {
        Task<ImportTimeSeriesResponse> BulkImportTimeSeries(CancellationToken cancellationToken);
        Task<ImportTimeSeriesResponse> ImportTimeSeries(List<TimeSeriesDto> timeSeries, EnergyUnit unit, CancellationToken cancellationToken);

    }
}
