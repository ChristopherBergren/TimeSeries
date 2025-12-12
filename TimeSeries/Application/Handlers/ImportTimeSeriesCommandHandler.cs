using MediatR;
using TimeSeries.Application.Commands;
using TimeSeries.Application.Responses;

namespace TimeSeries.Application.Handlers
{
    internal class ImportTimeSeriesCommandHandler()
        : IRequestHandler<ImportTimeSeriesCommand, ImportTimeSeriesResponse>
    {
        public async Task<ImportTimeSeriesResponse> Handle(ImportTimeSeriesCommand command, CancellationToken cancellationToken)
        {
            var response = new ImportTimeSeriesResponse();

            return response;
        }
    }
}