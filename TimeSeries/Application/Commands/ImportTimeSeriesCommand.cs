using MediatR;
using TimeSeriesRoot.Application.Models;
using TimeSeriesRoot.Application.Responses;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.Commands
{
    public record ImportTimeSeriesCommand : IRequest<ImportTimeSeriesResponse>
    {
        /// <summary>
        /// Measurement unit
        /// Valid values: MWh, kWh (case-insensitive)
        /// </summary>
        public EnergyUnit Unit { get; set; }
        /// <summary>
        /// List of TimeSeriesDto
        /// </summary>
        public List<TimeSeriesDto>? TimeSeries { get; set; }
    }
}
