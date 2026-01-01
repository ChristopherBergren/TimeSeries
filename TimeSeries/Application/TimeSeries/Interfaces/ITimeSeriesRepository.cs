using TimeSeriesRoot.Application.TimeSeries.Models;

namespace TimeSeriesRoot.Application.TimeSeries.Interfaces
{
    public interface ITimeSeriesRepository
    {
        Task<DbImportResult> ImportTimeSeriesAsync(List<TimeSeriesDto> timeSeries, CancellationToken cancellationToken);
        Task<List<TimeSeriesDto>> GetTimeSeriesAsync(int page, int pagesize);
        Task<List<TimeSeriesDto>> GetTimeSeriesByIdAsync(int seriesId);
        Task<List<TimeSeriesDto>> GetTimeSeriesInPeriodAsync(int seriesId, DateTime from, DateTime to);
        Task<int> GetNextSeriesIdsAsync();
    }
}
