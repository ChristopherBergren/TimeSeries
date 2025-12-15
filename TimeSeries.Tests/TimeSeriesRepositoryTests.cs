using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TimeSeriesRoot.Application.Models;
using TimeSeriesRoot.Infrastructure;
using TimeSeriesRoot.Infrastructure.Repositories;

namespace TimeSeriesRoot.Tests 
{
    public class TimeSeriesRepositoryTests
    {
        [Fact]
        public async Task ImportTimeSeriesAsync_Should_InsertAndMerge_UniqueRows()
        {
            // Arrange

            // Använder in-memory SQLite då komponenten med BulkMerge inte hanterar EF Core’s in-memory-db
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            await using var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var repository = new TimeSeriesRepository(context);

            var timestamp = DateTime.Now;
            var input = new List<TimeSeriesDto>
        {
            new() { Mba = "SE1", Timestamp = timestamp, TimestampUTC = timestamp.ToUniversalTime(), Quantity = 1.1, MgaCode = "ALS", MgaName = "Alingsås" },
            new() { Mba = "SE1", Timestamp = timestamp, TimestampUTC = timestamp.ToUniversalTime(), Quantity = 2.2, MgaCode = "ALS", MgaName = "Alingsås" }, // dubblett
            new() { Mba = "SE1", Timestamp = timestamp, TimestampUTC = timestamp.ToUniversalTime(), Quantity = 3.3, MgaCode = "AMS", MgaName = "Almnäs" } // unik
        };

            // Act
            DbImportResult dbImportResult = await repository.ImportTimeSeriesAsync(input, CancellationToken.None);

            // Assert

            // Verifiera merge av ALS-posterna
            var dbRows = await context.TimeSeries.ToListAsync();
            Assert.Equal(2, dbRows.Count); // 1 ALS + 1 AMS

            // Verifiera att ALS-raden uppdaterades med sista Quantity-värdet
            var alsRow = dbRows.First(x => x.MgaCode == "ALS");
            Assert.Equal(2.2, alsRow.Quantity);

            var amsRow = dbRows.First(x => x.MgaCode == "AMS");
            Assert.Equal(3.3, amsRow.Quantity);

            // Clean up
            await connection.CloseAsync();
        }
    }
}
