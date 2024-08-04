using Dapper;
using Hiwu.SpecificationPattern.Core.DataTransferObjects.Product;
using Hiwu.SpecificationPattern.Core.Interfaces.Repositories;
using Hiwu.SpecificationPattern.Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace Hiwu.SpecificationPattern.Domain.Repositories
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
                .QueryAsync<ProductCategoryDto>("SELECT * FROM View_HIWU_ProductCategoryView");
        }
    }
}
