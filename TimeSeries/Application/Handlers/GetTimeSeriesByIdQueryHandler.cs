using MediatR;
using TimeSeriesRoot.Application.Interfaces;
using TimeSeriesRoot.Application.Queries;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Handlers
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