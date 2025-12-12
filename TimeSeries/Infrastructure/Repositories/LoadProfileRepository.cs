using Castle.Core.Resource;
using TimeSeries.Application.Interfaces;
using TimeSeries.Application.Models;
using Z.EntityFramework.Extensions;

namespace TimeSeries.Infrastructure.Repositories
{
    public class LoadProfileRepository : ILoadProfileRepository
    {
        private readonly AppDbContext _context;
        public LoadProfileRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task UpsertLoadProfileAsync(List<TimeSeriesDto timeSeries, CancellationToken cancellationToken)
        {

            await _context.BulkMergeAsync(timeSeries, options => options.ColumnPrimaryKeyExpression = c => new { c.Timestamp, c.Mba }, cancellationToken);
            await _context.SaveChangesAsync();
        }

    }
}
