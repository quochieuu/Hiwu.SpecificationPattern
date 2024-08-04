using Hiwu.SpecificationPattern.Core.DataTransferObjects.Product;

namespace Hiwu.SpecificationPattern.Core.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductCategoryDto>> GetProductsWithCategoryAsync();
    }
}
