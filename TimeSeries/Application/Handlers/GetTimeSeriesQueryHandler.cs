using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using TimeSeries.Application.Models;
using TimeSeries.Application.Queries;
using TimeSeries.Application.Responses;

namespace TimeSeries.Application.Handlers
{
    internal class GetTimeSeriesQueryHandler
        : IRequestHandler<GetTimeSeriesQuery, GetTimeSeriesResponse>
    {
        private readonly IValidator<TimeSeriesDto> _timeSeriesValidator;

        public GetTimeSeriesQueryHandler(IValidator<TimeSeriesDto> validator) {
            _timeSeriesValidator = validator;
        }

        public async Task<GetTimeSeriesResponse> Handle(GetTimeSeriesQuery query, CancellationToken cancellationToken)
        {


        var validOrders = new List<TimeSeriesDto>();
            var failedOrders = new List<(TimeSeriesDto Order, string Error)>();

            foreach (var entry in query.TimeSeries!)
            {
                var result = await _timeSeriesValidator.ValidateAsync(entry, cancellationToken);
                if (result.IsValid)
                {
                    validOrders.Add(entry);
                }
                else
                {
                    var errorMessages = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                    failedOrders.Add((entry, errorMessages));
                }
            }





            var response = new GetTimeSeriesResponse();

            return response;
        }
    }
}