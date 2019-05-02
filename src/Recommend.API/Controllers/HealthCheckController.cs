using Microsoft.AspNetCore.Mvc;

namespace Recommend.API.Controllers
{
    [Route("[Controller]")]//不加，404
    public class HealthCheckController : Controller
    {
        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}