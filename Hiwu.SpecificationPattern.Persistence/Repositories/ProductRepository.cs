using Dapper;
using Hiwu.SpecificationPattern.Application.DataTransferObjects.Product;
using Hiwu.SpecificationPattern.Application.Interfaces.Repositories;
using Hiwu.SpecificationPattern.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hiwu.SpecificationPattern.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductsWithCategoryAsync()
        {
            return await _context.Database.GetDbConnection()
                .QueryAsync<ProductCategoryDto>("SELECT * FROM \"View_HIWU_ProductCategoryView\"");
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@min_price", minPrice);
                parameters.Add("@max_price", maxPrice);

                var products = await connection.QueryAsync<ProductCategoryDto>(
                    "\"SP_HIWU_GetProductsByPriceRange\"",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return products;
            }
        }

    }
}
