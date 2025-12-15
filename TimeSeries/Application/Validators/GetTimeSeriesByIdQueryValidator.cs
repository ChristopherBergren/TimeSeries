using FluentValidation;
using TimeSeriesRoot.Application.Commands;
using TimeSeriesRoot.Application.Queries;
using TimeSeriesRoot.Domain.Enums;
namespace TimeSeriesRoot.Application.Validators
{
    public class GetTimeSeriesByIdQueryValidator : AbstractValidator<GetTimeSeriesByIdQuery>
    {
        public GetTimeSeriesByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("'{PropertyName}' must be a number greater than 0");
            RuleFor(x => x.Start)
                .Matches(@"^(?:[01]\d|2[0-3])(?::[0-5]\d(?::[0-5]\d)?)?$")
                .WithMessage("Time must be in format hh, hh:mm or hh:mm:ss (00–23, 00–59)."); 
            RuleFor(x => x.End)
                .Matches(@"^(?:[01]\d|2[0-3])(?::[0-5]\d(?::[0-5]\d)?)?$")
                .WithMessage("Time must be in format hh, hh:mm or hh:mm:ss (00–23, 00–59).");
        }
    }
}
