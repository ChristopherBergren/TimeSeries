using FluentValidation;
using System.Globalization;
using TimeSeriesRoot.Application.Common;
using TimeSeriesRoot.Application.Common.Validators;
using TimeSeriesRoot.Application.TimeSeries.Models;

namespace TimeSeriesRoot.Application.TimeSeries.Validators
{
    public class TimeSeriesValidator : AbstractValidator<TimeSeriesDto>
    {
        public TimeSeriesValidator()
        {
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Value is required.")
                .Must(BeAValidDouble).WithMessage("Value must be a valid number.")
                .Must(value => LessThanZero(value!)).WithMessage("Value must be less than 0.");
            RuleFor(x => x.TimestampUTC)
                .IsUtc();
            RuleFor(x => x.Timestamp)
                .IsNullOrDateTime();
            RuleFor(x => x.Mba)
                .NotEmpty()
                .IsValidMBA();
            RuleFor(x => x.MgaCode)
                .NotEmpty();
            RuleFor(x => x.MgaName)
                .NotEmpty();
        }
        private static bool BeAValidDouble(string? value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return ConversionHelper.TryParseFlexible(value, out _);
        }
        private static bool LessThanZero(string value)
        {
            return ConversionHelper.TryParseFlexible(value, out var d) && d < 0;
        }
    }
}
