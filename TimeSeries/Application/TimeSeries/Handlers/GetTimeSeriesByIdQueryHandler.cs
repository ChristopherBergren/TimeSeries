using MediatR;
using TimeSeriesRoot.Application.TimeSeries.Interfaces;
using TimeSeriesRoot.Application.TimeSeries.Queries;
using TimeSeriesRoot.Application.TimeSeries.Responses;

namespace TimeSeriesRoot.Application.TimeSeries.Handlers
{
    internal class GetTimeSeriesByIdQueryHandler(ITimeSeriesService getTimeSeriesService)
        : IRequestHandler<GetTimeSeriesByIdQuery, GetTimeSeriesByIdResponse>
    {
        public async Task<GetTimeSeriesByIdResponse> Handle(GetTimeSeriesByIdQuery query, CancellationToken cancellationToken)
        {
            var response = await getTimeSeriesService.GetTimeSeriesById(query.Id, query.Unit, query.Start, query.End);

            return response;
        }
    }
}