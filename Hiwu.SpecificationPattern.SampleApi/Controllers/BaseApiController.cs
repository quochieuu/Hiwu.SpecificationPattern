using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hiwu.SpecificationPattern.SampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
