using AutoMapper;
using Hiwu.SpecificationPattern.Application.DataTransferObjects.Product;
using Hiwu.SpecificationPattern.Application.Interfaces.Repositories;
using Hiwu.SpecificationPattern.Domain.Entities;
using Hiwu.SpecificationPattern.SampleApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hiwu.SpecificationPattern.SampleApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/product")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("products")]
        [Cached(2000)]
        public async Task<IActionResult> ProductsGet()
        {
            var result = await _unitOfWork.Repository<Product>().ListAllAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("product")]
        public async Task<IActionResult> ProductsPost(List<CreateProductRequest> productRequests)
        {
            var products = _mapper.Map<List<Product>>(productRequests);

            var result = await _unitOfWork.Repository<Product>().AddRangeAsync(products);

            await _unitOfWork.CompleteAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("product-w-category")]
        public async Task<IActionResult> ProductCategoriesGet()
        {
            var result = await _productRepository.GetProductsWithCategoryAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("products-price-range")]
        public async Task<IActionResult> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var products = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            return Ok(products);
        }

    }
}
