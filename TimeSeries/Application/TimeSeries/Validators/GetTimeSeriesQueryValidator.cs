using FluentValidation;
using TimeSeriesRoot.Application.TimeSeries.Queries;
using TimeSeriesRoot.Application.TimeSeries.Commands;
using TimeSeriesRoot.Domain.Enums;
namespace TimeSeriesRoot.Application.TimeSeries.Validators
{
    public class GetTimeSeriesQueryValidator : AbstractValidator<GetTimeSeriesQuery>
    {
        public GetTimeSeriesQueryValidator()
        {
            RuleFor(x => x.Page)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("'{PropertyName}' must be a number greater than 0");
            RuleFor(x => x.PageSize)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("'{PropertyName}' must be a number greater than 0");

        }
    }
}