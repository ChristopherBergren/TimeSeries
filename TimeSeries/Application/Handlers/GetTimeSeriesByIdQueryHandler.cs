using MediatR;
using TimeSeriesRoot.Application.Queries;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Handlers
{
    internal class GetTimeSeriesByIdQueryHandler()
        : IRequestHandler<GetTimeSeriesByIdQuery, GetTimeSeriesByIdResponse>
    {
        public async Task<GetTimeSeriesByIdResponse> Handle(GetTimeSeriesByIdQuery query, CancellationToken cancellationToken)
        {
            var response = new GetTimeSeriesByIdResponse();

            return response;
        }
    }
}