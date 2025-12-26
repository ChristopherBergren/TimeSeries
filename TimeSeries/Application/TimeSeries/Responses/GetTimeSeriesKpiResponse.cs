namespace TimeSeriesRoot.Application.TimeSeries.Responses
{
    public class GetTimeSeriesKpiResponse
    {
        public List<Kpi> Kpis { get; set; } 

    }
    public record Kpi(string Description, double Value);
}
