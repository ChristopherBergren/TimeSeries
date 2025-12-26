using MediatR;
using TimeSeriesRoot.Application.TimeSeries.Interfaces;
using TimeSeriesRoot.Application.TimeSeries.Responses;
using TimeSeriesRoot.Application.TimeSeries.Commands;

namespace TimeSeriesRoot.Application.TimeSeries.Handlers
{
    internal class ImportTimeSeriesCommandHandler(IImportService profileService)
                : IRequestHandler<ImportTimeSeriesCommand, ImportTimeSeriesResponse>
    {
        public async Task<ImportTimeSeriesResponse> Handle(ImportTimeSeriesCommand command, CancellationToken cancellationToken)
        {
            var response = await profileService.ImportTimeSeries(command.TimeSeries!, command.Unit, cancellationToken);

            return response;
        }
    }
}