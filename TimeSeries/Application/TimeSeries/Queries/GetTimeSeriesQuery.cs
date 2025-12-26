using MediatR;
using TimeSeriesRoot.Application.TimeSeries.Responses;

namespace TimeSeriesRoot.Application.TimeSeries.Queries
{
    public record GetTimeSeriesQuery : IRequest<GetTimeSeriesResponse>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
