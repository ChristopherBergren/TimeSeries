using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.Metrics;
using TimeSeriesRoot.Application.TimeSeries.Interfaces;
using TimeSeriesRoot.Application.TimeSeries.Models;
using TimeSeriesRoot.Domain.Entities;
using TimeSeriesRoot.Infrastructure;

namespace TimeSeriesRoot.Infrastructure.TimeSeries
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
            var timeSeries = new List<TimeSeriesRoot.Domain.Entities.TimeSeries>();

            foreach (var item in timeSeriesDto)
            {
                timeSeries.Add(new TimeSeriesRoot.Domain.Entities.TimeSeries
                {
                    SeriesId = item.SeriesId,
                    Mba = item.Mba!,
                    MgaCode = item.MgaCode!,
                    MgaName = item.MgaName!,
                    Quantity = item.GetQuantity(),
                    Timestamp = (DateTime)item.Timestamp!,
                    TimestampUTC = (DateTime)item.TimestampUTC!,
                    CreatedAt = DateTime.UtcNow, // Ignoreras vid update
                    UpdatedAt = DateTime.UtcNow, // Ignoreras vid insert
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
                options.IgnoreOnMergeUpdateExpression = c => new { c.SeriesId, c.CreatedAt };
                options.IgnoreOnMergeInsertExpression = c => new { c.UpdatedAt };
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
        public async Task<int> GetNextSeriesIdsAsync() => (await _context.TimeSeries.Select(s => (int?)s.SeriesId).MaxAsync() ?? 0) + 1;
    }
}
