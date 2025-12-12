using MediatR;
using TimeSeries.Application.Responses;

namespace TimeSeries.Application.Commands
{
     public record ImportTimeSeriesCommand : IRequest<ImportTimeSeriesResponse>
     {
     }
}
