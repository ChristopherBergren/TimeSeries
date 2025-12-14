using FluentValidation;
using TimeSeries.Application.Commands;
using TimeSeries.Application.Models;
using TimeSeries.Application.Queries;
using TimeSeries.Domain.Enums;
namespace TimeSeries.Application.Validators
{
    public class UpsertTimeSeriesCommandValidator : AbstractValidator<UpsertTimeSeriesCommand>
    {
        public UpsertTimeSeriesCommandValidator()
        {
            RuleFor(x => x.TimeSeries)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.Unit)
                .NotNull()
                .Must(unit => Enum.IsDefined(typeof(EnergyUnit), unit));
        }
    }
}