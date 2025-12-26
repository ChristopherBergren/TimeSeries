using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.TimeSeries.Models
{
    public record TimeSeriesData (EnergyUnit EnergyUnit, List<TimeSeriesDto> TimeSeries);
}
