using MediatR;
using TimeSeriesRoot.Application.Interfaces;
using TimeSeriesRoot.Application.Queries;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Handlers
{
    internal class GetTimeSeriesKpiQueryHandler(ITimeSeriesService getTimeSeriesService)
        : IRequestHandler<GetTimeSeriesKpiQuery, GetTimeSeriesKpiResponse>
    {
        public async Task<GetTimeSeriesKpiResponse> Handle(GetTimeSeriesKpiQuery query, CancellationToken cancellationToken)
        {
            var response = await getTimeSeriesService.GetTimeSeriesKpi(query.Id, query.PeriodStart, query.PeriodEnd);

            return response;
        }
    }
}