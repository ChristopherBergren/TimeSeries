using MediatR;
using TimeSeriesRoot.Application.Commands;
using TimeSeriesRoot.Application.Interfaces;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Handlers
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