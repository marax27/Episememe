using System.Threading.Tasks;
using Episememe.Api.Utilities;
using Episememe.Application.Features.Hello;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class HelloController : ControllerBase
    {
        private readonly ILogger<HelloController> _logger;
        private readonly IMediator _mediator;

        public HelloController(ILogger<HelloController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var query = HelloQuery.Create(User.GetUserId());
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
