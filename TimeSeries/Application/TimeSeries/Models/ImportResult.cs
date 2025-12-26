namespace TimeSeriesRoot.Application.TimeSeries.Models
{
    public class ImportResult(bool success, int readCount = 0, int updateCount = 0, int insertCount = 0, int failedCount = 0)
    {
        public bool Success { get; set; } = success;
        public int ReadCount { get; set; } = readCount;
        public int UpdateCount { get; set; } = updateCount;
        public int InsertCount { get; set; } = insertCount;
        public int FailedCount { get; set; } = failedCount;
    }
}
