using MediatR;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Queries
{
    public record GetTimeSeriesKpiQuery : IRequest<GetTimeSeriesKpiResponse>
    {
    }
}
