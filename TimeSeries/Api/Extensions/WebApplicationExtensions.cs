using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Sinks.Seq;

namespace TimeSeries.Api.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using TimeSeries.Infrastructure;

    public static class WebApplicationExtensions
    {
        public static WebApplication InitializeDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();

            // Seed data 
            //if (!db.TodoItems.Any())
            //{
            //    db.TodoItems.AddRange(
            //        new TodoItem { Name = "Buy milk", IsDone = false },
            //        new TodoItem { Name = "Walk the dog", IsDone = true }
            //    );
            //    db.SaveChanges();
            //}

            return app;
        }
    }
}
