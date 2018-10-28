using Microsoft.EntityFrameworkCore;
namespace DuckFeeding.Models
{
    public class DuckFeedingContext : DbContext
    {
        public DuckFeedingContext(DbContextOptions<DuckFeedingContext> options)
            : base(options)
        {
        }

        public DbSet<DuckFeedingRecord> DuckFeedingRecords { get; set; }
    }
}
