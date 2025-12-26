using Microsoft.EntityFrameworkCore;
using TimeSeriesRoot.Domain.Entities;
namespace TimeSeriesRoot.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Entities.TimeSeries> TimeSeries => Set<TimeSeriesRoot.Domain.Entities.TimeSeries>();
        public DbSet<Domain.Entities.SeriesIdCounter> SeriesIdCounter => Set<SeriesIdCounter>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSeriesRoot.Domain.Entities.TimeSeries>()
                .HasKey(lp => lp.Id);
            modelBuilder.Entity<SeriesIdCounter>()
                .HasKey(lp => lp.Id);
        }
    }
}
