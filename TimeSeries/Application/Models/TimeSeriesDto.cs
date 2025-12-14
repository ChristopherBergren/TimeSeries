namespace TimeSeriesRoot.Application.Models
{
    public class TimeSeriesDto
    {
        // Min tolkning av serie-id (det som används i uppgift 2.2) är 
        // ett unikt gemensamt grupp-id som tilldelas samtliga datapunkter vid ett importtillfälle i uppgift 1.1, 
        // eller per importerad csv i uppgift 1.2
        public Guid SeriesId { get; set; }
        public string? Mba { get; set; }
        public string? MgaCode { get; set; }
        public string? MgaName { get; set; }
        public double? Quantity { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime? TimestampUTC { get; set; }
    }
}
