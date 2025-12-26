using FluentValidation;
using TimeSeriesRoot.Application.TimeSeries.BusinessRules;
using TimeSeriesRoot.Application.Common;

namespace TimeSeriesRoot.Application.Common.Validators
{
    public static class ValidationRuleExtensions
    {
        // Validera att indata är DateTime eller null
        public static IRuleBuilderOptions<T, DateTime?> IsNullOrDateTime<T>(
            this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder
                .Must(dt => !dt.HasValue || !dt.Equals(default(DateTime)))
                .WithMessage("{PropertyName} must be in UTC.");
        }
        // Validera att datum är i UTC
        public static IRuleBuilderOptions<T, DateTime> IsUtc<T>(
            this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .Must(dt => dt.Kind == DateTimeKind.Utc)
                .WithMessage("{PropertyName} must be in UTC.");
        }

        // Validera att datum är i UTC för nullable
        public static IRuleBuilderOptions<T, DateTime?> IsUtc<T>(
            this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder
                .Must(dt => dt.HasValue && dt.Value.Kind == DateTimeKind.Utc)
                .WithMessage("{PropertyName} must be in UTC if specified.");
        }
    }
}