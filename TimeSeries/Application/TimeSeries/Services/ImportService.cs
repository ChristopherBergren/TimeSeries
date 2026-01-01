using FluentValidation;
using Microsoft.Extensions.Options;
using TimeSeriesRoot.Application.TimeSeries.Interfaces;
using TimeSeriesRoot.Application.TimeSeries.Models;
using TimeSeriesRoot.Application.TimeSeries.Responses;
using TimeSeriesRoot.Application.TimeSeries.Imports;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.TimeSeries.Services
{
    public class ImportService : IImportService
    {
        private readonly Settings _settings;
        private readonly IValidator<TimeSeriesDto> _validator;
        private readonly ITimeSeriesRepository _repository;

        public ImportService(IOptions<Settings> settings, IValidator<TimeSeriesDto> validator, ITimeSeriesRepository repository)
        {
            _settings = settings.Value;
            _validator = validator;
            _repository = repository;
        }

        public async Task<ImportTimeSeriesResponse> BulkImportTimeSeries(CancellationToken cancellationToken)
        {
            var sourceFolder = _settings.CollectionPath;
            var pipeline = new BulkImportPipeline(_validator, _repository);
            var result = await pipeline.Execute(sourceFolder, cancellationToken);

            return new ImportTimeSeriesResponse(result);
        }

        public async Task<ImportTimeSeriesResponse> ImportTimeSeries(List<TimeSeriesDto> timeSeries, EnergyUnit unit, CancellationToken cancellationToken)
        {
            var processor = new TimeSeriesProcessor(_validator, _repository);
            var timeSeriesData = new TimeSeriesData(unit, timeSeries);

            // Validera tidsseriedata, konvertera till rätt enhet och lägg till serie-id
            var seriesId = await _repository.GetNextSeriesIdsAsync();
            var result = await processor.Process(timeSeriesData, seriesId, cancellationToken);
            // In i datalagret
            var dbImportResult = await _repository.ImportTimeSeriesAsync(result.ValidTimeSeries, cancellationToken);

            return new ImportTimeSeriesResponse(timeSeries.Count, dbImportResult.Updated, dbImportResult.Inserted, result.FailedCount);
        }
    }
}