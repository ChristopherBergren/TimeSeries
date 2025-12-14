using TimeSeries.Domain.Enums;

namespace TimeSeries.Application.Models
{
    public record TimeSeriesData (EnergyUnit EnergyUnit, List<TimeSeriesDto> TimeSeries);
}
