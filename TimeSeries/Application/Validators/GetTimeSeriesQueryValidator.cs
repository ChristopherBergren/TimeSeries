using FluentValidation;
using TimeSeriesRoot.Application.Commands;
using TimeSeriesRoot.Application.Queries;
using TimeSeriesRoot.Domain.Enums;
namespace TimeSeriesRoot.Application.Validators
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