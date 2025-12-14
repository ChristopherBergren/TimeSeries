using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Interfaces
{
    public interface ITimeSeriesService
    {
        Task<GetTimeSeriesResponse> GetTimeSeries(int page, int pageSize);
    }
}