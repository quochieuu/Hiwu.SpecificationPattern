using AutoMapper;
using Hiwu.SpecificationPattern.Application.DataTransferObjects.Product;
using Hiwu.SpecificationPattern.Application.Interfaces.Repositories;
using Hiwu.SpecificationPattern.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hiwu.SpecificationPattern.SampleApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/product")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> ProductsGet()
        {
            var result = await _unitOfWork.Repository.GetMultipleAsync<Product>(false);
            return Ok(result);
        }

        [HttpPost]
        [Route("product")]
        public async Task<IActionResult> ProductsPost(List<CreateProductRequest> productRequests)
        {
            var products = _mapper.Map<List<Product>>(productRequests);

            var result = await _unitOfWork.Repository.AddRangeAsync(products);

            await _unitOfWork.Repository.CompleteAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("product-w-category")]
        public async Task<IActionResult> ProductCategoriesGet()
        {
            var result = await _unitOfWork.ProductRepository.GetProductsWithCategoryAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("products-price-range")]
        public async Task<IActionResult> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            return Ok(products);
        }

    }
}
