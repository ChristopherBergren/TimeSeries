using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using TimeSeriesRoot.Application.Common.Behaviors;
using TimeSeriesRoot.Api.Extensions;
using TimeSeriesRoot.Infrastructure;
using TimeSeriesRoot.Application.TimeSeries.Models;
using TimeSeriesRoot.Application.TimeSeries.Interfaces;
using TimeSeriesRoot.Application.TimeSeries.Services;
using TimeSeriesRoot.Infrastructure.TimeSeries;
using TimeSeriesRoot.Application.TimeSeries.Validators;
using TimeSeriesRoot.Application.TimeSeries.BusinessRules;

namespace TimeSeriesRoot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var isDevelopment = builder.Environment.IsDevelopment();

            // Använd SeriLog (extension-metod)
            builder.UseCustomSerilog();

            // Sqlite
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=app.db"));

            builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

            // Initialize business-rules
            var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
            TimeSeriesBusinessRules.Initialize(settings!);

            builder.Services.AddScoped<ITimeSeriesRepository, TimeSeriesRepository>();
            builder.Services.AddScoped<IImportService, ImportService>();
            builder.Services.AddScoped<ITimeSeriesService, TimeSeriesService>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true));
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            builder.Services.AddSwaggerGen(c =>
            {
               c.EnableAnnotations();
            });

            // Registrera validering och MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
            builder.Services.AddValidatorsFromAssemblyContaining<ImportTimeSeriesCommandValidator>();
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
