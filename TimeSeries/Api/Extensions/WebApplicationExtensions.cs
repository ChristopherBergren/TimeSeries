namespace TimeSeriesRoot.Api.Extensions
{
    using TimeSeriesRoot.Infrastructure;

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
