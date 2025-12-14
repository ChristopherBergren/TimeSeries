//using Castle.Core.Resource;
//using FluentValidation;
//using Microsoft.Extensions.Options;
//using Serilog;
//using TimeSeries.Application.Imports;
//using TimeSeries.Application.Interfaces;
//using TimeSeries.Application.Models;
//using TimeSeries.Application.Responses;
//using TimeSeries.Domain.Enums;
//using TimeSeries.Infrastructure;

//namespace TimeSeries.Application.Services
//{
//    public class LoadProfileService : ILoadProfileService
//    {
//        private readonly Settings _settings;
//        private readonly IValidator<TimeSeriesDto> _validator;
//        private readonly ILoadProfileRepository _repository;

//        public LoadProfileService(
//            IOptions<Settings> settings,
//            IValidator<TimeSeriesDto> validator,
//            ILoadProfileRepository repository)
//        {
//            _settings = settings.Value; 
//            _validator = validator;
//            _repository = repository;
//        }

//        public async Task<UpsertTimeSeriesResponse> ImportTimeSeries(List<TimeSeriesDto> timeSeries, EnergyUnit unit, CancellationToken cancellationToken)
//        {
//            var processor = new TimeSeriesProcessor(_validator);
//            var timeSeriesData = new TimeSeriesData(unit, timeSeries);

//            // Validera tidsseriedata och konvertera till rätt enhet
//            var result = await processor.Process(timeSeriesData, cancellationToken);
//            // In i datalagret
//            var upsertResult = await _repository.UpsertLoadProfileAsync(result.ValidTimeSeries, cancellationToken);

//            return new UpsertTimeSeriesResponse(timeSeries.Count, upsertResult.Inserted, upsertResult.Updated, result.FailedCount);
//        }
//    }
//}
