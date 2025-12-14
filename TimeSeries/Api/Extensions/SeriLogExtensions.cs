using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace TimeSeriesRoot.Api.Extensions
{
    public static class SerilogExtensions
    {
        public static WebApplicationBuilder UseCustomSerilog(this WebApplicationBuilder builder)
        {
            bool isDevelopment = builder.Environment.IsDevelopment();

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Information();

            // ------------------------
            // Konfigurera per dev/övrig-miljö
            // ------------------------
            if (isDevelopment)
            {
                loggerConfig
                    // Utelämnar systemdata-loggningar för läsbarhet
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .WriteTo.Async(a => a.File(
                         new CompactJsonFormatter(),
                        "Logs/log-.json",
                        rollingInterval: RollingInterval.Day,
                        buffered: false,    // Ta bort buffring för direkt skrivning till loggen.
                        shared: false       // Snabbast, samt oftast ej nödvändigt med true här.
                                            // True om ex. Web Garden eller flera Docker containers delar en volym 
                                            // dvs. om flera processer ska dela loggningen
                    ));
            }
            else
            {
                // I prod kanske vi vill använda ex. en Seq-server istället för fil-loggning
                loggerConfig
                    .WriteTo.Async(a => a.Seq(
                        "http://localhost:5341",
                        batchPostingLimit: 1000,
                        queueSizeLimit: 100_000
                    ));
            }

            // Filtrera bort denna varning då det ej är prod-miljö (MediatR kräver numer licens i prod)
            loggerConfig.MinimumLevel.Override("LuckyPennySoftware.MediatR", Serilog.Events.LogEventLevel.Fatal);

            Log.Logger = loggerConfig.CreateLogger();
            builder.Host.UseSerilog();

            
            return builder;
        }
    }
}
