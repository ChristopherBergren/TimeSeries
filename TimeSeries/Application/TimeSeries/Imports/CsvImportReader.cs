using System.Globalization;
using System.Text.RegularExpressions;
using TimeSeriesRoot.Application.Common;
using TimeSeriesRoot.Application.TimeSeries.BusinessRules;
using TimeSeriesRoot.Application.TimeSeries.Models;
using TimeSeriesRoot.Domain.Enums;

namespace TimeSeriesRoot.Application.Imports
{
    public partial class CsvImportReader
    {
        [GeneratedRegex(@"(?<=\[).*(?=\])")]
        private static partial Regex UnitRegEx();
        private enum Fields { Date = 0, MBA = 1, MGA = 2, Quantity = 3 }
        private const int RequiredColumnCount = 4;
        private const string Delimiter = ";";


        public CsvImportReader() { }

        public async Task<ImportData> ImportTimeSeries(string filePath, CancellationToken cancellationToken)
        {
            var importData = await ProcessFile(filePath, cancellationToken);
            return importData;
        }

        private async Task<ImportData> ProcessFile(string filePath, CancellationToken cancellationToken)
        {
            var rows = new List<string>();
            EnergyUnit? energyUnit;

            // Läs in csv:n till lista
            using (var sr = new StreamReader(filePath, new FileStreamOptions { Access = FileAccess.Read, Mode = FileMode.Open }))
            {
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();
                    if (!string.IsNullOrEmpty(line?.Trim()))
                        rows.Add(line);
                }
            }

            // Försök hämta energi-enhet från Quantity-headern. Om den saknas utförs inte importen
            energyUnit = GetEnergyUnitFromHeader(rows[0]);
            if (energyUnit == null) 
                return new ImportData(false, null);

            // Konvertera till lista av TimeSeriesDto
            var timeSeries = await ProcessRows(rows);

            return new ImportData(true, new TimeSeriesData(energyUnit??EnergyUnit.kWh, timeSeries));
        }

        private async Task<List<TimeSeriesDto>> ProcessRows(List<string> rows)
        {
            var timeSeries = new List<TimeSeriesDto>();

            rows.RemoveAt(0);
            foreach (var line in rows)
            {
                var series = new TimeSeriesDto();
                (var valid, var columns) = TryGetColumns(line);
                
                if (valid)
                {
                    // Validera samtliga fält och lägg till om ok
                    var date = TimeSeriesBusinessRules.ConvertESettCsvDateToDateTime(columns![(int)Fields.Date]);
                    if (date!=null)
                    {
                        series.Timestamp = date;
                        series.TimestampUTC = ((DateTime)date!).ToUniversalTime();

                        var mba = columns[(int)Fields.MBA];
                        if (TimeSeriesBusinessRules.IsValidMba(mba))
                        {
                            series.Mba = mba;
                            series.MgaName = columns[(int)Fields.MGA];
                            series.MgaCode = series.MgaName; // Saknas i csv-filerna. Använd MgaName då fältet är obligatoriskt

                            if (ConversionHelper.TryParseFlexible(columns[(int)Fields.Quantity], out double quantity))
                            {
                                series.Quantity = quantity.ToString("G");

                                // Alla fält validerade, ok att lägga till
                                timeSeries.Add(series);
                            }
                        }
                    }
                }
            }

            return timeSeries;
        }

        private EnergyUnit? GetEnergyUnitFromHeader(string line)
        {
            (var success, var columns) = TryGetColumns(line);
            if (!success)
                return null;

            var quantityHeader = columns![(int)Fields.Quantity];
            return Enum.TryParse(UnitRegEx().Match(quantityHeader).Value, out EnergyUnit unit) ? unit : null;
        }

        private (bool lineValid, string[]? columns) TryGetColumns(string line)
        {
            var columns = line.Split(Delimiter);
            if (columns.Length != RequiredColumnCount)
                return (false, null);
            return (columns.All(c => !string.IsNullOrWhiteSpace(c)), columns);
        }
    }
}
