using Castle.Components.DictionaryAdapter.Xml;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using System.Text.Json.Serialization;
using TimeSeriesRoot.Application.Common;
using TimeSeriesRoot.Domain.Entities;
namespace TimeSeriesRoot.Application.TimeSeries.Models
{
    public class TimeSeriesDto
    {
        public TimeSeriesDto() { }
        public TimeSeriesDto(TimeSeriesRoot.Domain.Entities.TimeSeries timeSeries)        
        {
            SeriesId = timeSeries.SeriesId;
            Mba = timeSeries.Mba;
            MgaCode = timeSeries.MgaCode;
            MgaName = timeSeries.MgaName;
            Quantity = Math.Round(timeSeries.Quantity, 6).ToString("G");
            Timestamp = timeSeries.Timestamp;
            TimestampUTC = timeSeries.TimestampUTC;
            CreatedAt = timeSeries.CreatedAt;
            UpdatedAt = timeSeries.UpdatedAt;
        }

        public double GetQuantity()
        {

            if (!ConversionHelper.TryParseFlexible(Quantity, out double quantity))
                throw new InvalidCastException();

            return Math.Round(quantity, 6);
        }
        public override string ToString()
        {
            return $"[SeriesId:{SeriesId}, Mba:{Mba}, MgaCode:{MgaCode}, Quantity:{Quantity}, Timestamp:{Timestamp}, TimestampUTC:{TimestampUTC}]";
        }
        [SwaggerSchema(ReadOnly = true)] 
        public int SeriesId { get; set; }
        public string? Mba { get; set; }
        public string? MgaCode { get; set; }
        public string? MgaName { get; set; }
        public string? Quantity { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime? TimestampUTC { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
