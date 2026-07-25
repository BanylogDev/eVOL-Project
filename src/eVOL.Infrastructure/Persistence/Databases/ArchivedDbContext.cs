using eVOL.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eVOL.Infrastructure.Persistence.Databases
{
    public class ArchivedDbContext : DbContext
    {
        public ArchivedDbContext(DbContextOptions<ArchivedDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArchivedDbContext).Assembly);
        }
    }
}
