using TimeSeries.Application.Models;

namespace TimeSeries.Application.Validators
{
    public static class BusinessRules
    {
        private static BusinessRulesSettings? _rules;
        public static void Initialize(Settings settings)
        {
            _rules= settings.BusinessRules;
        }

        private static BusinessRulesSettings Rules => _rules ?? throw new InvalidOperationException("BusinessRules has not been initialized.");

        public static bool IsValidMba(string? mba)
        {
            return string.IsNullOrWhiteSpace(mba) || Rules.AllowedMbaValues.Contains(mba);
        }
    }
}
