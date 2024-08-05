using Hiwu.SpecificationPattern.Domain.Entities;
using Hiwu.SpecificationPattern.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hiwu.SpecificationPattern.Persistence.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}
