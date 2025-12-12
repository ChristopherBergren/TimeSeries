using FluentValidation;
using TimeSeries.Api.Extensions;
using TimeSeries.Application.Models;

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
                .NotNull()
                .IsUtc();
            RuleFor(x => x.Timestamp)
                .NotNull();
            RuleFor(x => x.Mba)
                .NotNull()
                .IsValidMBA();
            RuleFor(x => x.MgaCode)
                .NotNull()
                .NotEmpty();

        }
    }
}
