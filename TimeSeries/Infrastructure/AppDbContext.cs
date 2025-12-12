using Microsoft.EntityFrameworkCore;
using TimeSeries.Domain.Entities;
namespace TimeSeries.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<LoadProfile> LoadProfile => Set<LoadProfile>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoadProfile>()
                .HasKey(lp => lp.Id);
        }
    }
}
