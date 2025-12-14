using FluentValidation;
using TimeSeriesRoot.Application.Commands;
using TimeSeriesRoot.Domain.Enums;
namespace TimeSeriesRoot.Application.Validators
{
    public class ImportTimeSeriesCommandValidator : AbstractValidator<ImportTimeSeriesCommand>
    {
        public ImportTimeSeriesCommandValidator()
        {
            RuleFor(x => x.TimeSeries)
                .NotNull()
                .NotEmpty()
                .WithMessage("'{PropertyName}' must contain at least one entry");
            RuleFor(x => x.Unit)
                .NotNull()
                .Must(unit => Enum.IsDefined(typeof(EnergyUnit), unit))
                .WithMessage("'{PropertyName}' is not a valid unit. Possible values: MWh, kWh");
        }
    }
}