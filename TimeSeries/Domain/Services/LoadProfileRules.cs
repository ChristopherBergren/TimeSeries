namespace TimeSeries.Domain.Services
{
    public static class LoadProfileRules
    {
        private static readonly string[] AllowedMbaValues = { "SE1", "SE2", "SE3", "SE4" };

        // Validera värde på MBA
        // Hårdkodade värden här. Kan utökas till att hämta värdena från GET MBAOptions
        public static bool IsValidMba(string? mba)
        {
            return string.IsNullOrWhiteSpace(mba) || AllowedMbaValues.Contains(mba);
        }

        public static void EnsureValidMba(string? mba)
        {
            if (!IsValidMba(mba))
                throw new DomainException($"MBA value '{mba}' is invalid.");
        }
    }
}
