using MediatR;
using TimeSeries.Application.Queries;
using TimeSeries.Application.Responses;

namespace TimeSeries.Application.Handlers
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