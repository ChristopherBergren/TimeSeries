namespace TimeSeriesRoot.Application.TimeSeries.Models
{
    public record ProcessedTimeSeriesResult(List<TimeSeriesDto> ValidTimeSeries, int FailedCount);
}
