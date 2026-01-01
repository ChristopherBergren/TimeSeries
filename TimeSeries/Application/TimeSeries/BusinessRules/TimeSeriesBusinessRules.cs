using TimeSeriesRoot.Application.TimeSeries.Models;

namespace TimeSeriesRoot.Application.TimeSeries.BusinessRules
{
    public static class TimeSeriesBusinessRules
    {
        private static BusinessRulesSettings? _rules;
        public static void Initialize(Settings settings)
        {
            _rules= settings.BusinessRules;
        }

        private static BusinessRulesSettings Rules => _rules ?? throw new InvalidOperationException("BusinessRules has not been initialized.");

        public static bool IsValidMba(string? mba)
        {
            return string.IsNullOrWhiteSpace(mba) || Rules.AllowedMbaValues.Contains(mba.ToUpper());
        }

        public static DateTime? ConvertESettCsvDateToDateTime(string csvFileDateFormat)
        {
            // Datumformat i csv-filer från eSett: 27.11.2025/01:45
            // https://opendata.esett.com/load_profile
            try
            {
                var dateAndTime = csvFileDateFormat.Split("/");
                var hourMin = dateAndTime[1].Split(':');
                var dateParts = dateAndTime[0].Split(".");
                var dateTime = new DateTime(Convert.ToInt32(dateParts[2]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[0]), Convert.ToInt32(hourMin[0]), Convert.ToInt32(hourMin[1]), 0);
                return dateTime;
            }
            catch
            {
                return null;
            }
        }
    }
}
