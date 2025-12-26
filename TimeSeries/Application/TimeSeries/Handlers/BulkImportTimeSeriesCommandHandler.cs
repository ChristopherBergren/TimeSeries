using MediatR;
using TimeSeriesRoot.Application.TimeSeries.Interfaces;
using TimeSeriesRoot.Application.TimeSeries.Responses;
using TimeSeriesRoot.Application.TimeSeries.Commands;

namespace TimeSeriesRoot.Application.TimeSeries.Handlers
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