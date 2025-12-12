using MediatR;
using TimeSeries.Application.Queries;
using TimeSeries.Application.Responses;

namespace TimeSeries.Application.Handlers
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