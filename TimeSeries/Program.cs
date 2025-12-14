using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Text.Json;
using System.Text.Json.Serialization;
using TimeSeries.Api.Extensions;
using TimeSeries.Application.Behaviors;
using TimeSeries.Application.Interfaces;
using TimeSeries.Application.Models;
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

            builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

            // Initialize business-rules
            var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
            BusinessRules.Initialize(settings!);

            builder.Services.AddScoped<ILoadProfileRepository, LoadProfileRepository>();
            builder.Services.AddScoped<IImportService, ImportService>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true));
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            builder.Services.AddSwaggerGen();

            // Registrera validering och MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
            builder.Services.AddValidatorsFromAssemblyContaining<UpsertTimeSeriesCommandValidator>();
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            var app = builder.Build();

            // Aktivera SwaggerUI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
