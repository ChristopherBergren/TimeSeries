using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;
using TimeSeriesRoot.Application.Commands;
using TimeSeriesRoot.Application.Interfaces;
using TimeSeriesRoot.Application.Models;
using TimeSeriesRoot.Application.Responses;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.IntegrationTests
{
    public class TimeSeriesControllerEndToEndTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TimeSeriesControllerEndToEndTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Ersätt repot med en mock 
                    var repoDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ITimeSeriesRepository));
                    if (repoDescriptor != null) services.Remove(repoDescriptor);

                    var mockRepo = new Mock<ITimeSeriesRepository>();
                    mockRepo.Setup(r => r.ImportTimeSeriesAsync(
                         It.IsAny<List<TimeSeriesDto>>(),
                         It.IsAny<CancellationToken>()))
                         .ReturnsAsync((List<TimeSeriesDto> list, CancellationToken ct) =>
                         {
                             // Samma duplicerings-logik som BulkMerge:
                             // Unik nyckel: (Timestamp, Mba, MgaCode)

                             var groups = list
                                 .GroupBy(x => new { x.Timestamp, x.Mba, x.MgaCode })
                                 .ToList();

                             int inserted = 0;
                             int updated = 0;

                             foreach (var g in groups)
                             {
                                 inserted += 1;          // första i gruppen = insert
                                 updated += g.Count() - 1; // övriga = updates
                             }

                             return new DbImportResult(inserted, updated);
                         });


                    services.AddSingleton(mockRepo.Object);
                });
            });
        }

        [Theory]
        [MemberData(nameof(GetImportTestData))]
        public async Task PostParse_ShouldReturnCorrectCounts(List<TimeSeriesDto> input, int expectedRead, int expectedInsert, int expectedUpdate, int expectedFailed)
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new ImportTimeSeriesCommand { TimeSeries = input, Unit = EnergyUnit.MWh };

            // Act
            var response = await client.PostAsJsonAsync("/api/timeseries/parse", command);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ImportTimeSeriesResponse>(
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRead, result.ReadCount);
            Assert.Equal(expectedInsert, result.InsertCount);
            Assert.Equal(expectedUpdate, result.UpdateCount);
            Assert.Equal(expectedFailed, result.FailedCount);
        }

        // Test-data
        public static IEnumerable<object[]> GetImportTestData()
        {
            var timestamp = DateTime.UtcNow;

            // Scenario 1: 3 giltiga, 1 ogiltig (Quantity > 0)
            var input1 = new List<TimeSeriesDto>
        {
            new() { Mba = "SE1", MgaCode = "ALS", MgaName = "Alingsås", Quantity = -2.0, Timestamp = timestamp, TimestampUTC = timestamp },
            new() { Mba = "SE1", MgaCode = "ALS", MgaName = "Alingsås", Quantity = -3.0, Timestamp = timestamp, TimestampUTC = timestamp }, // duplicate
            new() { Mba = "SE1", MgaCode = "AMS", MgaName = "Almnäs", Quantity = -2.5, Timestamp = timestamp, TimestampUTC = timestamp },
            new() { Mba = "SE99", MgaCode = "AMS", MgaName = "Almnäs", Quantity = 2.0, Timestamp = timestamp, TimestampUTC = timestamp } // invalid
        };
            yield return new object[] { input1, 4, 2, 1, 1 };

            // Scenario 2: alla giltiga
            var input2 = new List<TimeSeriesDto>
        {
            new() { Mba = "SE1", MgaCode = "ALS", MgaName = "Alingsås", Quantity = -1.0, Timestamp = timestamp, TimestampUTC = timestamp },
            new() { Mba = "SE2", MgaCode = "AMS", MgaName = "Almnäs", Quantity = -2.0, Timestamp = timestamp, TimestampUTC = timestamp }
        };
            yield return new object[] { input2, 2, 2, 0, 0 };

            // Scenario 3: alla ogiltiga (nulls eller Quantity >= 0)
            var input3 = new List<TimeSeriesDto>
        {
            new() { Mba = null, MgaCode = "", MgaName = "", Quantity = 1.0, Timestamp = null, TimestampUTC = null },
            new() { Mba = "SE100", MgaCode = "XXX", MgaName = "", Quantity = 0, Timestamp = timestamp, TimestampUTC = timestamp }
        };
            yield return new object[] { input3, 2, 0, 0, 2 };
        }
    }
}
