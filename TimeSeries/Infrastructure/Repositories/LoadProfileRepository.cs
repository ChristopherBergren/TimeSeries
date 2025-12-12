using Castle.Core.Resource;
using TimeSeries.Application.Interfaces;
using TimeSeries.Application.Models;
using TimeSeries.Domain.Entities;
using Z.BulkOperations;
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
        public async Task<UpsertResult> UpsertLoadProfileAsync(List<TimeSeriesDto> timeSeries, CancellationToken cancellationToken)
        {
            var loadProfile = new List<LoadProfile>();

            // Kunde använt AutoMapper här
            foreach (var item in timeSeries)
            {
                loadProfile.Add(new LoadProfile
                {
                    Mba = item.Mba!,
                    MgaCode = item.MgaCode!,
                    MgaName = item.MgaName!,
                    Quantity = (double)item.Quantity!,
                    Timestamp = (DateTime)item.Timestamp!,
                    TimestampUTC = (DateTime)item.TimestampUTC!
                });
            }

            // Notera att detta är en metod i Z.EntityFramework.Extensions, som är en licensbelagd 3:e-partskomponent.
            // Den är dock extensivt använd, speciellt för stora mängder db-operation, där EF Core helt enkelt är för långsamt.
            // Jag valde [c.Timestamp, c.Mba, c.MgaCode] som unik nyckel i Upsert-operationen. 
            var resultInfo = new Z.BulkOperations.ResultInfo();
            await _context.BulkMergeAsync(loadProfile, options =>
            {
                options.ColumnPrimaryKeyExpression = c => new { c.Timestamp, c.Mba, c.MgaCode };
                options.UseRowsAffected = true;
                options.ResultInfo = resultInfo;
            }, cancellationToken);

            return new UpsertResult(resultInfo.RowsAffectedInserted, resultInfo.RowsAffectedUpdated );
        }

    }
}
