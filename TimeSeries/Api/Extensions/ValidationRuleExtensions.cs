using FluentValidation;
using FluentValidation.Validators;
using System;

namespace TimeSeries.Api.Extensions
{
    public static class ValidationRuleExtensions
    {
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
                .Must(dt => !dt.HasValue || dt.Value.Kind == DateTimeKind.Utc)
                .WithMessage("{PropertyName} must be in UTC if specified.");
        }

        // Validera värde på MBA
        // Hårdkodade värden här. Kan utökas till att hämta värdena från GET MBAOptions
        public static IRuleBuilderOptions<T, string?> IsValidMBA<T>(
          this IRuleBuilder<T, string?> ruleBuilder)
        {
            string[] stringArray = { "SE1", "SE2", "SE3", "SE4" };
            return ruleBuilder
                .Must(s => string.IsNullOrWhiteSpace(s) || stringArray.Any(s.Contains))
                .WithMessage("{PropertyName} has an invalid value.");
        }
    }
}