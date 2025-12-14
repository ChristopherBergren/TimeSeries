using MediatR;
using TimeSeriesRoot.Application.Responses;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.Queries
{
    public record GetTimeSeriesByIdQuery : IRequest<GetTimeSeriesByIdResponse>
    {
        public required string Id { get; set; }
        public EnergyUnit Unit { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}
