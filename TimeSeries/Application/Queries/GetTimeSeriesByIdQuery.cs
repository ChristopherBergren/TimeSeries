using MediatR;
using TimeSeriesRoot.Application.Responses;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.Queries
{
    public record GetTimeSeriesByIdQuery : IRequest<GetTimeSeriesByIdResponse>
    {
        public int Id { get; set; }
        public EnergyUnit Unit { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
    }
}
