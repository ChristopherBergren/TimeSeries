using FluentValidation;
using MediatR;
using TimeSeries.Application.Commands;
using TimeSeries.Application.Interfaces;
using TimeSeries.Application.Models;
using TimeSeries.Application.Responses;

namespace TimeSeries.Application.Handlers
{
    internal class UpsertTimeSeriesCommandHandler(ILoadProfileService profileService)
                : IRequestHandler<UpsertTimeSeriesCommand, UpsertTimeSeriesResponse>
    {
        public async Task<UpsertTimeSeriesResponse> Handle(UpsertTimeSeriesCommand command, CancellationToken cancellationToken)
        {
            var response = await profileService.UpsertTimeSeries(command.TimeSeries!, command.Unit, cancellationToken);

            return response;
        }
    }
}