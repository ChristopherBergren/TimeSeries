using MediatR;
using TimeSeriesRoot.Application.Queries;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Handlers
{
    internal class GetTimeSeriesKpiQueryHandler
        : IRequestHandler<GetTimeSeriesKpiQuery, GetTimeSeriesKpiResponse>
    {
        public async Task<GetTimeSeriesKpiResponse> Handle(GetTimeSeriesKpiQuery query, CancellationToken cancellationToken)
        {
            var response = new GetTimeSeriesKpiResponse();

            return response;
        }
    }
}