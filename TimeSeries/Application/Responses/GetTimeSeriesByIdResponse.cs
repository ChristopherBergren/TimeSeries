using TimeSeriesRoot.Application.Models;

namespace TimeSeriesRoot.Application.Responses
{
    public class GetTimeSeriesByIdResponse
    {
        public List<TimeSeriesDto> TimeSeries { get; set; } = [];
    }
}
