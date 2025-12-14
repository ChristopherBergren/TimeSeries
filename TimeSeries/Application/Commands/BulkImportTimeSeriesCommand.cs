using MediatR;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Commands
{
     public record BulkImportTimeSeriesCommand : IRequest<ImportTimeSeriesResponse>
     {
     }
}
