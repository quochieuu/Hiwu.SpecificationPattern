using Hiwu.SpecificationPattern.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hiwu.SpecificationPattern.SampleApi.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Nike",
                    Content = "New fashion 2021",
                    Price = 120000,
                    UrlImage = "nike.png"
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Adidas",
                    Content = "Adidas modern 2024",
                    Price = 120000,
                    UrlImage = "adidas.png"
                }
            );
        }
    }
}
