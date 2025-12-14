using FluentValidation;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Moq;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using TimeSeries.Application.Models;
using TimeSeries.Application.Validators;
using TimeSeries.Domain.Enums;

namespace TimeSeries.Application.Imports
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
            using (var reader = new StreamReader(filePath, new FileStreamOptions { Access = FileAccess.Read, Mode = FileMode.Open }))
            {
                var line = reader.ReadLine();
                if(!string.IsNullOrEmpty(line?.Trim()))
                    rows.Add(line);
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
                    if (DateTime.TryParse(columns![(int)Fields.Date], out DateTime date))
                    {
                        series.Timestamp = date;
                        series.TimestampUTC = date.ToUniversalTime();

                        var mba = columns[(int)Fields.MBA];
                        if (BusinessRules.IsValidMba(mba))
                        {
                            series.Mba = mba;
                            series.MgaName = columns[(int)Fields.MGA];
                            series.MgaCode = "[Saknas]"; // Saknas i csv-filerna

                            if (double.TryParse(columns[(int)Fields.Quantity].Replace(",", "."), out double quantity))
                            {
                                series.Quantity = quantity;

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
