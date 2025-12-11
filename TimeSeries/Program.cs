using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using TimeSeries.Extensions;

namespace TimeSeries
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var isDevelopment = builder.Environment.IsDevelopment();

            // Använd SeriLog (olika settings beroende på miljö)
            builder.UseCustomSerilog();

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger().UseSwaggerUI();
            }

            //app.UseAuthorization();
            app.MapControllers();

            // Se till att buffrad loggning skrivs till fil när app avslutas
            app.Lifetime.ApplicationStopping.Register(() => Log.CloseAndFlush());

            app.Run();
        }
    }
}
