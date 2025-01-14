using Microsoft.EntityFrameworkCore;

using Phonebook.Directory.Domain;

namespace Phonebook.Directory.Persistence
{
    public class DirectoryDbContext : DbContext
    {
        public DirectoryDbContext(DbContextOptions<DirectoryDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSnakeCaseNamingConvention();
    }
}
