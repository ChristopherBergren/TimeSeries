using MediatR;
using TimeSeriesRoot.Application.Interfaces;
using TimeSeriesRoot.Application.Queries;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Handlers
{
    internal class GetTimeSeriesQueryHandler(ITimeSeriesService getTimeSeriesService)
        : IRequestHandler<GetTimeSeriesQuery, GetTimeSeriesResponse>
    {
        public async Task<GetTimeSeriesResponse> Handle(GetTimeSeriesQuery query, CancellationToken cancellationToken)
        {
            var response = await getTimeSeriesService.GetTimeSeries(query.Page, query.PageSize);

            return response;
        }
    }
}