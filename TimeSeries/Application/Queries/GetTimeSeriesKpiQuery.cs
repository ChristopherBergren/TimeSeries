using MediatR;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Queries
{
    public record GetTimeSeriesKpiQuery : IRequest<GetTimeSeriesKpiResponse>
    {
        public int Id { get; set; } 
        public required string PeriodStart { get; set; }
        public required string PeriodEnd { get; set; }
    }
}
