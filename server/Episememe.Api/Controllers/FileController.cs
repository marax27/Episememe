using System.Threading.Tasks;
using Episememe.Application.Features.FileMedia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IMediator _mediator;

        public FileController(ILogger<FileController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("files/{id}")]
        public async Task<IActionResult> GetLoadedMedia(string id)
        {
            var query = FileMediaQuery.Create(id);
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
