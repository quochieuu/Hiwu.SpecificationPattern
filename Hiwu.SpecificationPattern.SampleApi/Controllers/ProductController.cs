using Hiwu.SpecificationPattern.Generic;
using Hiwu.SpecificationPattern.SampleApi.Context;
using Hiwu.SpecificationPattern.SampleApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hiwu.SpecificationPattern.SampleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
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
    }
}
