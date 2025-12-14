using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.Models
{
    public record TimeSeriesData (EnergyUnit EnergyUnit, List<TimeSeriesDto> TimeSeries);
}
