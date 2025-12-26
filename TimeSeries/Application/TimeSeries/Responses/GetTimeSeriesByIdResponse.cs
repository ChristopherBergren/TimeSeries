using TimeSeriesRoot.Application.TimeSeries.Models;

namespace TimeSeriesRoot.Application.TimeSeries.Responses
{
    public class GetTimeSeriesByIdResponse
    {
        public List<TimeSeriesDto> TimeSeries { get; set; } = [];
    }
}
