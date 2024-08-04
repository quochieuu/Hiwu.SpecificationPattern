using Hiwu.SpecificationPattern.Core.Entities;
using Hiwu.SpecificationPattern.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Hiwu.SpecificationPattern.SampleApi.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
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
        public async Task<IActionResult> ProductsPost()
        {
            var result = await _unitOfWork.Repository.AddRangeAsync(new List<Product> ()
            {
                new ()
                {
                    Name = "Iphone 15 promax",
                    Content = "Iphone",
                    Price = 30000,
                    UrlImage = "ip15.png"
                },
                new ()
                {
                    Name = "Iphone 14 promax",
                    Content = "Iphone",
                    Price = 20000,
                    UrlImage = "ip14.png"
                }
            });

            await _unitOfWork.Repository.CompleteAsync();
            return Ok(result);
        }
    }
}
