using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.Metrics;
using TimeSeriesRoot.Application.Interfaces;
using TimeSeriesRoot.Application.Models;
using TimeSeriesRoot.Domain.Entities;

namespace TimeSeriesRoot.Infrastructure.Repositories
{
    public class TimeSeriesRepository : ITimeSeriesRepository
    {
        private readonly AppDbContext _context;
        
        public TimeSeriesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<DbImportResult> ImportTimeSeriesAsync(List<TimeSeriesDto> timeSeriesDto, CancellationToken cancellationToken)
        {
            var timeSeries = new List<Domain.Entities.TimeSeries>();

            foreach (var item in timeSeriesDto)
            {
                timeSeries.Add(new TimeSeries
                {
                    SeriesId = item.SeriesId,
                    Mba = item.Mba!,
                    MgaCode = item.MgaCode!,
                    MgaName = item.MgaName!,
                    Quantity = (double)item.Quantity!,
                    Timestamp = (DateTime)item.Timestamp!,
                    TimestampUTC = (DateTime)item.TimestampUTC!
                });
            }

            // Notera att detta är en metod i Z.EntityFramework.Extensions, som är en licensbelagd 3:e-partskomponent.
            // https://entityframework-extensions.net/bulk-merge
            // Den är dock extensivt använd, speciellt för stora mängder db-operationer, där EF Core helt enkelt är för långsamt,
            // samt för att EF Core helt enkelt saknar merge-funktionalitet.
            var resultInfo = new Z.BulkOperations.ResultInfo();
            await _context.BulkMergeAsync(timeSeries, options =>
            {
                // Som Composite Key för att avgöra unikhet valde jag Timestamp/Mba/MgaCode
                options.ColumnPrimaryKeyExpression = c => new { c.Timestamp, c.Mba, c.MgaCode };
                // Om datapunkt existerar skall vi uppdatera, men SeriesId skall behålla originalvärdet (min tolkning)
                options.IgnoreOnMergeUpdateExpression = c => new { c.SeriesId };
                // Denna switch gör operationen något långsammare, men krävs för att få tillbaka antal inserts/updates
                options.UseRowsAffected = true;
                options.ResultInfo = resultInfo;
            }, cancellationToken);

            return new DbImportResult(resultInfo.RowsAffectedInserted, resultInfo.RowsAffectedUpdated );
        }
        public async Task<List<TimeSeriesDto>> GetTimeSeriesAsync(int page, int pagesize)
        {
            var totalCount = _context.TimeSeries.Count();
            var startIndex = --page * pagesize;
            if (startIndex < totalCount)
            {
                var count = Math.Min(totalCount - startIndex, pagesize);
                // Valde att sortera i fallande datum-ordning. Utan retention börjar sida 1 med väldigt gammal data.
                var series = _context.TimeSeries
                    .OrderByDescending(s => s.Timestamp)
                    .Skip(startIndex)
                    .Take(count);
                var timeSeriesDto = series.Select(s => new TimeSeriesDto(s)).ToList();

                return timeSeriesDto;
            }
            return [];
        }
        public async Task<List<TimeSeriesDto>> GetTimeSeriesByIdAsync(int seriesId)
        {
            return _context.TimeSeries
                .OrderBy(s => s.Timestamp)
                .Where(s => s.SeriesId == seriesId)
                .Select(s => new TimeSeriesDto(s)).ToList();
        }
        public async Task<List<TimeSeriesDto>> GetTimeSeriesInPeriodAsync(int seriesId, DateTime from, DateTime to)
        {
            return _context.TimeSeries
                .Where(s => s.SeriesId == seriesId && s.Timestamp >= from && s.Timestamp <= to)
                .Select(s => new TimeSeriesDto(s)).ToList();
        }
        public async Task<int> GetNextSeriesIdsAsync(int noOfIds)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            await _context.SeriesIdCounter.Where(c => c.Id == 1)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.LatestSeriesId, c => c.LatestSeriesId + noOfIds));
            await _context.SaveChangesAsync();
            var lastSeriesId = await _context.SeriesIdCounter
                .Where(c => c.Id == 1)
                .Select(c => c.LatestSeriesId)
                .SingleAsync();
            
            await transaction.CommitAsync();

            return lastSeriesId;
        }
    }
}
