using FluentValidation;
using TimeSeries.Application.Models;
using TimeSeries.Application.Queries;
namespace TimeSeries.Application.Validators
{
    public class GetTimeSeriesQueryValidator : AbstractValidator<GetTimeSeriesQuery>
    {
        public GetTimeSeriesQueryValidator()
        {
            RuleFor(x => x.TimeSeries)
                .NotNull()
                .NotEmpty();

            // Validate each item in the collection using the child validator
            //RuleForEach(x => x.Orders).SetValidator(new OrderDtoValidator());
        }
    }
    public class TimeSeriesDtoValidator : AbstractValidator<TimeSeriesDto>
    {
        public TimeSeriesDtoValidator()
        {
            RuleFor(x => x.Value).LessThan(0);
        }
    }
}