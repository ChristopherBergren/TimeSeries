using MediatR;
using TimeSeriesRoot.Application.TimeSeries.Responses;

namespace TimeSeriesRoot.Application.TimeSeries.Commands
{
     public record BulkImportTimeSeriesCommand : IRequest<ImportTimeSeriesResponse>
     {
     }
}
