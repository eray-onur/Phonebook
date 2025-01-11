using Microsoft.EntityFrameworkCore;

using Phonebook.Report.Domain;

namespace Phonebook.Report.Persistence
{
    public class PhonebookDbContext : DbContext
    {
        public DbSet<PhonebookReport> Reports { get; set; }
        public PhonebookDbContext(DbContextOptions<PhonebookDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSnakeCaseNamingConvention();
    }
}
