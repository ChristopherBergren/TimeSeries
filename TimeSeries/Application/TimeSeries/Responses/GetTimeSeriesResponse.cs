using TimeSeriesRoot.Application.TimeSeries.Models;

namespace TimeSeriesRoot.Application.TimeSeries.Responses
{
    public class GetTimeSeriesResponse
    {
        public List<TimeSeriesDto> TimeSeries { get; set; } = [];
    }
}
