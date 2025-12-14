using TimeSeries.Application.Models;

namespace TimeSeries.Application.Responses
{
    public class UpsertTimeSeriesResponse(int readCount, int updateCount, int insertCount, int failedCount)
    {
        public UpsertTimeSeriesResponse(ImportResult importResult) 
            : this (importResult.ReadCount, importResult.UpdateCount, importResult.InsertCount, importResult.FailedCount) { }

        public int ReadCount { get; set; } = readCount;
        public int UpdateCount { get; set; } = updateCount;
        public int InsertCount { get; set; } = insertCount;
        public int FailedCount { get; set; } = failedCount;
    }
}
