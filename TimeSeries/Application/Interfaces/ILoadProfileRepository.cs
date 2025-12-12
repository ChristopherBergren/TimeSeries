using TimeSeries.Application.Models;
using static TimeSeries.Infrastructure.Repositories.LoadProfileRepository;

namespace TimeSeries.Application.Interfaces
{
    public interface ILoadProfileRepository
    {
        Task<UpsertResult> UpsertLoadProfileAsync(List<TimeSeriesDto> timeSeries, CancellationToken cancellationToken);
    }
}
