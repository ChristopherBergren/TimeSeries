using FluentValidation;
using TimeSeriesRoot.Application.TimeSeries.BusinessRules;
using TimeSeriesRoot.Application.Common;

namespace TimeSeriesRoot.Application.Common.Validators
{
    public static class TimeSeriesValidationRuleExtensions
    {
        // Validera värde på MBA
        public static IRuleBuilderOptions<T, string?> IsValidMBA<T>(
          this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Must(mba => TimeSeriesBusinessRules.IsValidMba(mba))
                .WithMessage("{PropertyName} has an invalid value.");
        }
    }
}