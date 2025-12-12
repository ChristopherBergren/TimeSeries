using MediatR;
using TimeSeries.Application.Models;
using TimeSeries.Application.Responses;

namespace TimeSeries.Application.Queries
{
    public record GetTimeSeriesQuery : IRequest<GetTimeSeriesResponse>
    {
        public List<TimeSeriesDto>? TimeSeries { get; set; }
    }
}
