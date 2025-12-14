using MediatR;
using TimeSeries.Application.Models;
using TimeSeries.Application.Responses;
using TimeSeries.Domain.Enums;

namespace TimeSeries.Application.Queries
{
    public record GetTimeSeriesQuery : IRequest<GetTimeSeriesResponse>
    {
        public EnergyUnit Unit { get; set; }
        public List<TimeSeriesDto>? TimeSeries { get; set; }
    }
}
