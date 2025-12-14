namespace TimeSeriesRoot.Domain.Entities
{
    public class TimeSeries
    {
        // Min tolkning av SeriesId (det som används i uppgift 2.2) är 
        // ett unikt gemensamt grupp-id som tilldelas samtliga datapunkter vid ett importtillfälle i uppgift 1.1, 
        // eller per importerad csv i uppgift 1.2

        public Guid SeriesId { get; set; }
        public int Id { get; set; }
        public string Mba { get; set; } = string.Empty;
        public string MgaCode { get; set; }=string.Empty;
        public string MgaName { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime TimestampUTC { get; set; }

        public void AddItem(string mba, string mgaCode, string mgaName, double quantity, DateTime timestamp, DateTime timestampUtc)
        {
            if (quantity >= 0) throw new DomainException("Quantity must be negative");
            
            Mba = mba;
            MgaCode = mgaCode;
            MgaName = mgaName;
            Quantity = quantity;
            Timestamp = timestamp;
            TimestampUTC = timestampUtc;
        }
    }
}
