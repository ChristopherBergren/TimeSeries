using FluentValidation;
using TimeSeriesRoot.Application.TimeSeries.Queries;
using TimeSeriesRoot.Application.TimeSeries.Commands;
using TimeSeriesRoot.Domain.Enums;
namespace TimeSeriesRoot.Application.TimeSeries.Validators
{
    public class GetTimeSeriesKpiQueryValidator : AbstractValidator<GetTimeSeriesKpiQuery>
    {
        public GetTimeSeriesKpiQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("'{PropertyName}' must be a number greater than 0");
            RuleFor(x => x.PeriodStart)
                .Matches(@"^\d{4}-\d{2}-\d{2}$")
                .WithMessage("Date must be in format yyyy-MM-dd.");
            RuleFor(x => x.PeriodEnd)
                .Matches(@"^\d{4}-\d{2}-\d{2}$")
                .WithMessage("Date must be in format yyyy-MM-dd.");
        }
    }
}
