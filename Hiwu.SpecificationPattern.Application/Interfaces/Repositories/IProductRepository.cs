using Hiwu.SpecificationPattern.Application.DataTransferObjects.Product;

namespace Hiwu.SpecificationPattern.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductCategoryDto>> GetProductsWithCategoryAsync();
        Task<IEnumerable<ProductCategoryDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}
