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

            return app;
        }
    }
}
