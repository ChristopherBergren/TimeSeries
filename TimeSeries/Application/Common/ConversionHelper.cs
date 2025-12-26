using System.Globalization;

namespace TimeSeriesRoot.Application.Common
{
    public static class ConversionHelper
    {
        public static bool TryParseFlexible(string? input, out double value)
        {
            value = default;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            return double.TryParse(input?.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }
    }
}
