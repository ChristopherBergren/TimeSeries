using MediatR;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Queries
{
    public record GetTimeSeriesQuery : IRequest<GetTimeSeriesResponse>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
