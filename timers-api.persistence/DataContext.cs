using Microsoft.EntityFrameworkCore;
using timers_api.domain.Entities;

namespace timers_api.persistence
{
    public class DataContext : DbContext
    {
        private const string Schema = "dbo";

        protected DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var spotTrade = modelBuilder.Entity<TimerEntity>();
            spotTrade.HasKey(x => x.Id);
            //spotTrade.HasIndex(t => t.Timestamp).IsUnique(false);
            //spotTrade.Property(x => x.Id).ValueGeneratedOnAdd();
            spotTrade.Property(x => x.WebhookUrl).HasMaxLength(2048);
            spotTrade.ToTable("timers", Schema);
        }

        public DbSet<TimerEntity> Timers { get; set; }
    }
}
