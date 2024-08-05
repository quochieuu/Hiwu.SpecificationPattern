using Dapper;
using Hiwu.SpecificationPattern.Application.DataTransferObjects.Product;
using Hiwu.SpecificationPattern.Application.Interfaces.Repositories;
using Hiwu.SpecificationPattern.Persistence.Database;
using Microsoft.EntityFrameworkCore;

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
    }
}
