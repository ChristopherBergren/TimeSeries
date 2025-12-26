using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Moq;
using TimeSeriesRoot.Application.TimeSeries.Interfaces;
using TimeSeriesRoot.Application.TimeSeries.Models;
using TimeSeriesRoot.Application.TimeSeries.Services;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Tests
{
    public class TimeSeriesServiceTests
    {
        [Theory]
        [MemberData(nameof(GetTimeSeriesTestData))]
        public async Task ImportTimeSeries_Should_CallRepositoryCorrectly_AndReturnCorrectCounts(
            List<TimeSeriesDto> input,
            List<TimeSeriesDto> expectedValidEntries,
            int expectedInserted,
            int expectedUpdated)
        {
            // Arrange
            var mockValidator = new Mock<IValidator<TimeSeriesDto>>();
            var mockRepo = new Mock<ITimeSeriesRepository>();
            var mockOptions = new Mock<IOptions<Settings>>();

            // Mock validator: markera enbart expectedValidEntries som giltiga
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TimeSeriesDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TimeSeriesDto dto, CancellationToken _) =>
                {
                    if (expectedValidEntries.Any(x => x.Mba == dto.Mba && x.MgaCode == dto.MgaCode))
                        return new ValidationResult(); // giltig

                    return new ValidationResult(new List<ValidationFailure>
                    {
                        new ValidationFailure("Mba", "Invalid")
                    });
                });

            // Mock repository: simulera insert/update-logig som verkliga BulkMerge
            mockRepo.Setup(r => r.ImportTimeSeriesAsync(It.IsAny<List<TimeSeriesDto>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<TimeSeriesDto> list, CancellationToken ct) =>
                {
                    var groups = list.GroupBy(x => new { x.Timestamp, x.Mba, x.MgaCode });
                    int inserted = 0, updated = 0;
                    foreach (var g in groups)
                    {
                        inserted += 1;                // första i gruppen = insert
                        updated += g.Count() - 1;     // övriga = updates
                    }
                    return new DbImportResult(inserted, updated);
                });

            var service = new ImportService(mockOptions.Object, mockValidator.Object, mockRepo.Object);
            // Act
            var response = await service.ImportTimeSeries(input, EnergyUnit.MWh, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.ImportTimeSeriesAsync(
                It.Is<List<TimeSeriesDto>>(list => list.Count == expectedValidEntries.Count),
                It.IsAny<CancellationToken>()),
                Times.Once);

            Assert.Equal(input.Count, response.ReadCount);           // total input
            Assert.Equal(expectedInserted, response.InsertCount);    // inserts
            Assert.Equal(expectedUpdated, response.UpdateCount);     // updates
            Assert.Equal(input.Count - expectedValidEntries.Count, response.FailedCount); // ogiltiga
        }

        public static IEnumerable<object[]> GetTimeSeriesTestData()
        {
            var timestamp = System.DateTime.UtcNow;

            // Scenario 1: 3 giltiga, 1 ogiltig, en dubblett → 2 insert, 1 update
            var input1 = new List<TimeSeriesDto>
            {
                new() { Mba = "SE1", MgaCode = "ALS", Quantity = "-2.345", Timestamp = timestamp },
                new() { Mba = "SE1", MgaCode = "ALS", Quantity = "-3.345", Timestamp = timestamp }, // dubblett
                new() { Mba = "SE1", MgaCode = "AMS", Quantity = "-2.345", Timestamp = timestamp },
                new() { Mba = "SE99", MgaCode = "AMS", Quantity = "-2.345", Timestamp = timestamp } // ogiltig
            };
            var expected1 = new List<TimeSeriesDto>
            {
                input1[0], input1[1], input1[2]
            };
            yield return new object[] { input1, expected1, 2, 1 }; // 2 insert, 1 update

            // Scenario 2: alla giltiga, inga dubbletter → 2 insert, 0 update
            var input2 = new List<TimeSeriesDto>
            {
                new() { Mba = "SE1", MgaCode = "ALS", Quantity = "-1.0", Timestamp = timestamp },
                new() { Mba = "SE2", MgaCode = "AMS", Quantity = "-2.0", Timestamp = timestamp }
            };
            var expected2 = new List<TimeSeriesDto>(input2);
            yield return new object[] { input2, expected2, 2, 0 };

            // Scenario 3: alla ogiltiga → 0 insert, 0 update
            var input3 = new List<TimeSeriesDto>
            {
                new() { Mba = "SE1", MgaCode = "ALS", Quantity = "1.0", Timestamp = timestamp },
                new() { Mba = "SE2", MgaCode = "AMS", Quantity = "2.0", Timestamp = timestamp }
            };
            var expected3 = new List<TimeSeriesDto>(); // inga giltiga rader
            yield return new object[] { input3, expected3, 0, 0 };

            // Scenario 4: alla ogiltiga → 0 insert, 0 update
            var input4 = new List<TimeSeriesDto>
            {
                new() { Mba = "SE99", MgaCode = "ALS", Quantity = "-1.0", Timestamp = timestamp },
                new() { Mba = "SE100", MgaCode = "AMS", Quantity = "-2.0", Timestamp = timestamp }
            };
            var expected4 = new List<TimeSeriesDto>(); // inga giltiga rader
            yield return new object[] { input4, expected4, 0, 0 };
        }
    }
}