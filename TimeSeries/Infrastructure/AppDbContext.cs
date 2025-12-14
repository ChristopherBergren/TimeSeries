using Microsoft.EntityFrameworkCore;
using TimeSeriesRoot.Domain.Entities;
namespace TimeSeriesRoot.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Entities.TimeSeries> TimeSeries => Set<TimeSeries>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSeries>()
                .HasKey(lp => lp.Id);
        }
    }
}
