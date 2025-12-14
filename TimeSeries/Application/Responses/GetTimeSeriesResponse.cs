using TimeSeriesRoot.Application.Models;

namespace TimeSeriesRoot.Application.Responses
{
    public class GetTimeSeriesResponse
    {
        public List<TimeSeriesDto> TimeSeries { get; set; } = [];
    }
}
