using MediatR;
using TimeSeriesRoot.Application.TimeSeries.Responses;

namespace TimeSeriesRoot.Application.TimeSeries.Queries
{
    public record GetTimeSeriesKpiQuery : IRequest<GetTimeSeriesKpiResponse>
    {
        public int Id { get; set; } 
        public required string PeriodStart { get; set; }
        public required string PeriodEnd { get; set; }
    }
}
