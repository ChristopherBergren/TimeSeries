using MediatR;
using TimeSeriesRoot.Application.TimeSeries.Interfaces;
using TimeSeriesRoot.Application.TimeSeries.Queries;
using TimeSeriesRoot.Application.TimeSeries.Responses;

namespace TimeSeriesRoot.Application.TimeSeries.Handlers
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