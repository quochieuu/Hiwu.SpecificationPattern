using Hiwu.SpecificationPattern.Core.Entities;
using Hiwu.SpecificationPattern.SampleApi.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hiwu.SpecificationPattern.Domain.Database
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
