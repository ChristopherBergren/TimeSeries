using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TimeSeries.Application.Imports;
using TimeSeries.Application.Interfaces;
using TimeSeries.Application.Models;
using TimeSeries.Application.Services;
using TimeSeries.Domain.Enums;
using Xunit;

namespace TimeSeries.Tests
{
    public class CsvTests
    {
        [Fact]
        public async Task Test()
        {
            var csv = new CsvImportReader();

            //var list = csv.GetTimeSeries("D:\\Dev\\FILES\\TimeSeries\\Export-2025-12-11T22_48_06.csv", new CancellationToken());

        }
    }
}