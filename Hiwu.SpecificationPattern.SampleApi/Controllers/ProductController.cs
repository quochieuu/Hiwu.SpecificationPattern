using AutoMapper;
using Hiwu.SpecificationPattern.Core.DataTransferObjects.Product;
using Hiwu.SpecificationPattern.Core.Entities;
using Hiwu.SpecificationPattern.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Hiwu.SpecificationPattern.SampleApi.Controllers
{
    public class ProductController : BaseApiController
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
    }
}
