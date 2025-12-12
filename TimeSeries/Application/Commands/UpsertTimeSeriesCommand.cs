using MediatR;
using TimeSeries.Application.Models;
using TimeSeries.Application.Responses;
using TimeSeries.Domain.Enums;

namespace TimeSeries.Application.Commands
{
    public record UpsertTimeSeriesCommand : IRequest<UpsertTimeSeriesResponse>
    {
        public MeasurementUnit Unit { get; set; }
        public List<TimeSeriesDto>? TimeSeries { get; set; }
    }
}
