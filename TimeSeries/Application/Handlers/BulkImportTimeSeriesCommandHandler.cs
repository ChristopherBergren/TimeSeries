using MediatR;
using TimeSeriesRoot.Application.Commands;
using TimeSeriesRoot.Application.Interfaces;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Handlers
{
    internal class BulkImportTimeSeriesCommandHandler(IImportService profileService)
        : IRequestHandler<BulkImportTimeSeriesCommand, ImportTimeSeriesResponse>
    {
        public async Task<ImportTimeSeriesResponse> Handle(BulkImportTimeSeriesCommand command, CancellationToken cancellationToken)
        {
            var response = await profileService.BulkImportTimeSeries(cancellationToken);
            return response;
        }
    }
}