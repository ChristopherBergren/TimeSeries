using FluentValidation;
using TimeSeries.Api.Extensions;
using TimeSeries.Application.Models;
using TimeSeries.Domain.Enums;

namespace TimeSeries.Application.Validators
{
    public class TimeSeriesValidator : AbstractValidator<TimeSeriesDto>
    {
        public TimeSeriesValidator()
        {
            RuleFor(x => x.Quantity)
                .NotNull()
                .LessThan(0);
            RuleFor(x => x.TimestampUTC)
                .IsUtc();
            RuleFor(x => x.Timestamp)
                .IsNullOrDateTime();
            RuleFor(x => x.Mba)
                .NotNull()
                .IsValidMBA();
            RuleFor(x => x.MgaCode)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.MgaName)
                .NotNull()
                .NotEmpty();

        }
    }
}
