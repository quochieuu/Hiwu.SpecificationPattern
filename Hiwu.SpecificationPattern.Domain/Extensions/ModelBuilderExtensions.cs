using Hiwu.SpecificationPattern.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hiwu.SpecificationPattern.SampleApi.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var catFashion = Guid.NewGuid();
            var catShoe = Guid.NewGuid();

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = catFashion,
                    Name = "Fashion"
                },
                new Category()
                {
                    Id = catShoe,
                    Name = "Shoes"
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Summer T-shirt fashion 2024",
                    Content = "New fashion 2024",
                    Price = 120000,
                    UrlImage = "summer.png",
                    CategoryId = catFashion
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Adidas",
                    Content = "Adidas modern 2024",
                    Price = 120000,
                    UrlImage = "adidas.png",
                    CategoryId = catShoe
                }
            );
        }
    }
}
