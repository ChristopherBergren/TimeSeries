using Newtonsoft.Json.Linq;
using System.Diagnostics.Eventing.Reader;
using TimeSeries.Domain.Enums;

namespace TimeSeries.Domain.Entities
{
    public class LoadProfile
    {
        public int Id { get; set; }
        public string Mba { get; set; } = string.Empty;
        public string MgaCode { get; set; }=string.Empty;
        public string MgaName { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime TimestampUTC { get; set; }
        public double Total { get; set; }

        public void AddItem(string mba, string mgaCode, string mgaName, double quantity, DateTime timestamp, DateTime timestampUtc, double total)
        {
            if (quantity >= 0) throw new DomainException("Quantity must be negative");
            
            Mba = mba;
            MgaCode = mgaCode;
            MgaName = mgaName;
            Quantity = quantity;
            Timestamp = timestamp;
            TimestampUTC = timestampUtc;
            Total = total;
        }
    }
}
