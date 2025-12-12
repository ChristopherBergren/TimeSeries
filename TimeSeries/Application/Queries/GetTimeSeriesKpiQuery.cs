using MediatR;
using TimeSeries.Application.Responses;

namespace TimeSeries.Application.Queries
{
    public record GetTimeSeriesKpiQuery : IRequest<GetTimeSeriesKpiResponse>
    {
    }
}
