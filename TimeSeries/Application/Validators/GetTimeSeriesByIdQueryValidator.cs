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
            RuleFor(x => x.Unit)
                .NotNull()
                .NotEmpty()
                .Must(unit => Enum.IsDefined(typeof(EnergyUnit), unit))
                .WithMessage("'{PropertyName}' is not a valid unit. Possible values: MWh, kWh");

            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .Must(id => Guid.TryParse(id, out _))
                .WithMessage("'{PropertyName}' is not a valid GUID format.");
        }
    }
}