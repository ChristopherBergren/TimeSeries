using TimeSeriesRoot.Application.TimeSeries.Responses;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.TimeSeries.Interfaces
{
    public interface ITimeSeriesService
    {
        Task<GetTimeSeriesResponse> GetTimeSeries(int page, int pageSize);
        Task<GetTimeSeriesByIdResponse> GetTimeSeriesById(int seriesId, EnergyUnit unit, string start, string end);
        Task<GetTimeSeriesKpiResponse> GetTimeSeriesKpi(int seriesId, string periodStart, string periodEnd);
    }
}