namespace TimeSeries.Domain.Entities
{
    public class LoadProfile
    {
        public int Id { get; set; }
        public string Mba { get; set; }
        public string MgaCode { get; set; }
        public string MgaName { get; set; }
        public double Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime TimestampUTC { get; set; }
        public double Total { get; set; }
    }
}
