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

            // Räknare för att sätta SeriesId i TimeSeries vid importer. Endast en post
            if (!db.SeriesIdCounter.Any())
            {
                db.SeriesIdCounter.Add(new Domain.Entities.SeriesIdCounter { LatestSeriesId = 0 });
                db.SaveChanges();
            }

            return app;
        }
    }
}
