using TimeSeriesRoot.Application.Models;

namespace TimeSeriesRoot.Application.Responses
{
    public class ImportTimeSeriesResponse(int readCount, int updateCount, int insertCount, int failedCount)
    {
        public ImportTimeSeriesResponse()
            : this(0, 0, 0, 0) { }
        public ImportTimeSeriesResponse(ImportResult importResult) 
            : this (importResult.ReadCount, importResult.UpdateCount, importResult.InsertCount, importResult.FailedCount) { }

        public int ReadCount { get; set; } = readCount;
        public int UpdateCount { get; set; } = updateCount;
        public int InsertCount { get; set; } = insertCount;
        public int FailedCount { get; set; } = failedCount;
    }
}
