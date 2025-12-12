namespace TimeSeries.Application.Models
{
    public class TimeSeriesDto
    {
        public string? Mba { get; set; }
        public string? MgaCode { get; set; }
        public string? MgaName { get; set; }
        public double? Quantity { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime? TimestampUTC { get; set; }
        public double? Total { get; set; }
    }
}
