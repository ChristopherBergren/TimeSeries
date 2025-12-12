namespace TimeSeries.Application.Responses
{
    public class UpsertTimeSeriesResponse
    {
        public int ReadCount { get; set; }
        public int UpdateCount { get; set; }
        public int InsertCount { get; set; }
        public int FailedCount { get; set; }
    }
}
