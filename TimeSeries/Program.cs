using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Text.Json;
using System.Text.Json.Serialization;
using TimeSeries.Api.Extensions;
using TimeSeries.Application.Behaviors;
using TimeSeries.Application.Interfaces;
using TimeSeries.Application.Services;
using TimeSeries.Application.Validators;
using TimeSeries.Infrastructure;
using TimeSeries.Infrastructure.Repositories;

namespace TimeSeries
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var isDevelopment = builder.Environment.IsDevelopment();

            // Använd SeriLog (extension-metod)
            builder.UseCustomSerilog();

            // Jag valde Sqlite som db
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=app.db"));

            builder.Services.AddScoped<ILoadProfileRepository, LoadProfileRepository>();
            builder.Services.AddScoped<ILoadProfileService, LoadProfileService>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true)
                    );
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Registrera validering och MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
            builder.Services.AddValidatorsFromAssemblyContaining<UpsertTimeSeriesCommandValidator>();
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();

            app.UseSwagger().UseSwaggerUI();

            // Skapa databasen om den inte existerar samt applicera initial seedning (extension-metod)
            app.InitializeDatabase();

            app.MapControllers();

            // Se till att buffrad loggning skrivs till fil när app avslutas
            // I detta projekt använder jag dock inte buffring
            app.Lifetime.ApplicationStopping.Register(() => Log.CloseAndFlush());

            app.Run();
        }
    }
}
