using MediatR;
using TimeSeries.Application.Commands;
using TimeSeries.Application.Responses;

namespace TimeSeries.Application.Handlers
{
    internal class UpsertTimeSeriesCommandHandler()
        : IRequestHandler<UpsertTimeSeriesCommand, UpsertTimeSeriesResponse>
    {
        public async Task<UpsertTimeSeriesResponse> Handle(UpsertTimeSeriesCommand command, CancellationToken cancellationToken)
        {
            var response = new UpsertTimeSeriesResponse();

            return response;
        }
    }
}