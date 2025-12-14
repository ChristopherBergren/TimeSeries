using MediatR;
using TimeSeries.Application.Models;
using TimeSeries.Application.Responses;
using TimeSeries.Domain.Enums;

namespace TimeSeries.Application.Commands
{
    public record UpsertTimeSeriesCommand : IRequest<UpsertTimeSeriesResponse>
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
