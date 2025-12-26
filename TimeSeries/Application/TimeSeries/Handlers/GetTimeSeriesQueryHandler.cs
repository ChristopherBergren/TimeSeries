using MediatR;
using TimeSeriesRoot.Application.TimeSeries.Interfaces;
using TimeSeriesRoot.Application.TimeSeries.Queries;
using TimeSeriesRoot.Application.TimeSeries.Responses;

namespace TimeSeriesRoot.Application.TimeSeries.Handlers
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