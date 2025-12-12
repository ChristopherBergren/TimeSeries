using Microsoft.EntityFrameworkCore;
namespace TimeSeries.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<LoadProfileDTO> People => Set<LoadProfileDTO>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }

    public class LoadProfileDTO
    {
        public string Mba { get; set; }
        public string MgaCode { get; set; }
        public string MgaName { get; set; }
        public double Quantity { get; set; }
        public string Timestamp { get; set; }
        public string TimestampUTC { get; set; }
        public double Total { get; set; }
    }
}
