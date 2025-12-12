using Newtonsoft.Json.Linq;
using System.Diagnostics.Eventing.Reader;
using TimeSeries.Domain.Enums;
using TimeSeries.Domain.Services;

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

        public void AddItem(string? mba, string? mgaCode, string? mgaName, double? quantity, DateTime? timestamp, DateTime? timestampUtc, double? total)
        {
            if (quantity == null || quantity >= 0) throw new DomainException("Quantity must have a value and be negative");
            LoadProfileRules.EnsureValidMba(mba);

                        Mba = mba ?? string.Empty;
            MgaCode = mgaCode ?? string.Empty;
            MgaName = mgaName ?? string.Empty;
            Quantity = quantity ?? default;
            Timestamp = timestamp ?? default;
            TimestampUTC = timestampUtc ?? default;
            Total = total ?? default;
        }
    }
}
//dt.Kind == DateTimeKind.Utc)
