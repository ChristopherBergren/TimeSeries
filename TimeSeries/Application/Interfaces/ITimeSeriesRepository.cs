using TimeSeriesRoot.Application.Models;

namespace TimeSeriesRoot.Application.Interfaces
{
    public interface ITimeSeriesRepository
    {
        Task<DbImportResult> ImportTimeSeriesAsync(List<TimeSeriesDto> timeSeries, CancellationToken cancellationToken);
        Task<List<TimeSeriesDto>> GetTimeSeries(int page, int pagesize);
    }
}
