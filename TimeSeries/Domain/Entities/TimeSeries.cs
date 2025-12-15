namespace TimeSeriesRoot.Domain.Entities
{
    public class TimeSeries
    {
        public int SeriesId { get; set; }
        public int Id { get; set; }
        public string Mba { get; set; } = string.Empty;
        public string MgaCode { get; set; }=string.Empty;
        public string MgaName { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime TimestampUTC { get; set; }
    }
}
