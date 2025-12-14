using FluentValidation;
using Serilog;
using System.Collections.Concurrent;
using TimeSeriesRoot.Application.Interfaces;
using TimeSeriesRoot.Application.Models;

namespace TimeSeriesRoot.Application.Imports
{
    public class BulkImportPipeline
    {
        private static readonly SemaphoreSlim _entryLock = new(1, 1);
        private readonly IValidator<TimeSeriesDto> _validator;
        private readonly ITimeSeriesRepository _repository;

        public BulkImportPipeline(IValidator<TimeSeriesDto> validator, ITimeSeriesRepository repository)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ImportResult> Execute(string sourceFolder, CancellationToken cancellationToken)
        {
            await _entryLock.WaitAsync();
            ImportResult result;

            try
            {
                result = await ExecutePipeline(sourceFolder, cancellationToken);
            }
            finally
            {
                _entryLock.Release();
            }

            return result;
        }

        private async Task<ImportResult> ExecutePipeline(string path, CancellationToken cancellationToken)
        {
            var results = new ConcurrentBag<ProcessedTimeSeriesResult>();
            var csvImportReader = new CsvImportReader();
            var processor = new TimeSeriesProcessor(_validator);

            // Steg 1: Kontrollera om det finns filer att importera
            (bool hasFiles, string[]? files) = GetFilePaths(path);
            if (!hasFiles)
                return new ImportResult(false);

            // Steg 2: Läs in filerna och konvertera till internt format
            await Parallel.ForEachAsync(
                files!,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                },
                async (path, ct) =>
                {
                    ImportData importData = new ImportData(false,null);

                    switch (Path.GetExtension(path).ToLower())
                    {
                        case ".csv":
                            importData = await csvImportReader.ImportTimeSeries(path, ct);
                            break;
                        case ".json":
                            // importData = jsonImportReader.GetTimeSeries(path, ct));
                            break;
                        default:
                            return;
                    }

                    if (importData.IsValid)
                    {
                        // Validera tidsseriedata och konvertera till rätt enhet
                        var result = await processor.Process(importData.TimeSeriesData!, cancellationToken);

                        // Lägg till importen till ConcurrentBag
                        results.Add(result);
                    }
                });

            // Steg 3: Merga resultaten och sänd till repository
            var validTimeSeries = results.SelectMany(r => r.ValidTimeSeries).ToList();
            var failedCount = results.Sum(r => r.FailedCount);
            var dbImportResult = await _repository.ImportTimeSeriesAsync(validTimeSeries, cancellationToken);

            return new ImportResult(true, validTimeSeries.Count + failedCount, dbImportResult.Updated, dbImportResult.Inserted, failedCount);
        }

        private static (bool hasFiles, string[]? filePaths) GetFilePaths(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Log.Error($"Folder doesn't exist: {folder}");
                return (false, null);
            }

            var files = System.IO.Directory.GetFiles(folder, "*.*", new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = true });

            return ((files.Length > 0), files);
        }
    }
}
