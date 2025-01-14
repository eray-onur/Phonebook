using Microsoft.EntityFrameworkCore;

using Phonebook.Report.Domain;

namespace Phonebook.Report.Persistence
{
    public class ReportDbContext : DbContext
    {
        public DbSet<PhonebookReport> Reports { get; set; }
        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSnakeCaseNamingConvention();
    }
}
