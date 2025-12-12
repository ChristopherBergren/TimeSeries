namespace TimeSeries.Application.Interfaces
{
    public interface ILoadProfileRepository
    {
        Task UpsertLoadProfileAsync(CancellationToken cancellationToken);
    }
}
