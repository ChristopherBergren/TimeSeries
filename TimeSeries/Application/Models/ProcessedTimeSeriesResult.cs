namespace TimeSeriesRoot.Application.Models
{
    public record ProcessedTimeSeriesResult(List<TimeSeriesDto> ValidTimeSeries, int FailedCount);
}
