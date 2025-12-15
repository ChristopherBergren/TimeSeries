using TimeSeriesRoot.Domain.Entities;

namespace TimeSeriesRoot.Application.Models
{
    public class TimeSeriesDto
    {
        public TimeSeriesDto() { }
        public TimeSeriesDto(TimeSeries timeSeries)        
        {
            SeriesId = timeSeries.SeriesId;
            Mba = timeSeries.Mba;
            MgaCode = timeSeries.MgaCode;
            MgaName = timeSeries.MgaName;
            Quantity = Math.Round(timeSeries.Quantity, 6);
            Timestamp = timeSeries.Timestamp;
            TimestampUTC = timeSeries.Timestamp;
        }

        public int SeriesId { get; set; }
        public string? Mba { get; set; }
        public string? MgaCode { get; set; }
        public string? MgaName { get; set; }
        public double? Quantity { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime? TimestampUTC { get; set; }
    }
}
