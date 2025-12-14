using Microsoft.Extensions.Options;
using TimeSeriesRoot.Application.Interfaces;
using TimeSeriesRoot.Application.Models;
using TimeSeriesRoot.Application.Responses;

namespace TimeSeriesRoot.Application.Services
{
    public class TimeSeriesService : ITimeSeriesService
    {
        private readonly Settings _settings;
        private readonly ITimeSeriesRepository _repository;

        public TimeSeriesService(IOptions<Settings> settings, ITimeSeriesRepository repository)
        {
            _settings = settings.Value;
            _repository = repository;
        }

        public async Task<GetTimeSeriesResponse> GetTimeSeries(int page, int pageSize)
        {
            var timeSeriesDto = await _repository.GetTimeSeries(page, pageSize);
            var response = new GetTimeSeriesResponse { TimeSeries = timeSeriesDto };

            return response;
        }
    }
}