using System.Threading.Tasks;
using Episememe.Application.Features.FileMedia;
using Episememe.Application.Features.VerifyBrowseToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IMediator _mediator;

        public FilesController(ILogger<FilesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("files/{id}")]
        public async Task<IActionResult> GetFile(string id, [FromQuery] string token)
        {
            var tokenQuery = VerifyBrowseTokenQuery.Create(token);
            var isTokenValid = await _mediator.Send(tokenQuery);
            if (!isTokenValid)
                return Unauthorized();

            var fileQuery = FileMediaQuery.Create(id);
            var result = await _mediator.Send(fileQuery);
            return result;
        }
    }
}
