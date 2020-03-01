using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class HelloController : ControllerBase
    {
        private readonly ILogger<HelloController> _logger;

        public HelloController(ILogger<HelloController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello world!";
        }
    }
}
