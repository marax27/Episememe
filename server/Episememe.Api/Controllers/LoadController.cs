using Episememe.Application.DataTransfer;
using System.Threading.Tasks;
using Episememe.Api.Utilities;
using Episememe.Application.Features.LoadMedia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class LoadController : ControllerBase
    {
        private readonly ILogger<LoadController> _logger;
        private readonly IMediator _mediator;

        public LoadController(ILogger<LoadController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("files")]
        public async Task<IActionResult> GetSearchedMedia([FromRoute] LoadMediaDto loadMediaDto)
        {
            var query = LoadMediaQuery.Create(loadMediaDto);
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
