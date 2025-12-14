namespace TimeSeries.Application.Models
{
    public record ImportData(bool IsValid, TimeSeriesData? TimeSeriesData);
}
