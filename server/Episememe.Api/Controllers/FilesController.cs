using Episememe.Api.Utilities;
using Episememe.Application.DataTransfer;
using Episememe.Application.Features.FileMedia;
using Episememe.Application.Features.FileUpload;
using Episememe.Application.Features.VerifyBrowseToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
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

            var fileQuery = FileMediaQuery.Create(id, User.GetUserId());
            var result = await _mediator.Send(fileQuery);
            return result;
        }

        [HttpPost]
        [Authorize]
        [Route("files")]
        public async Task<IActionResult> UploadNewFile([FromForm] FileUploadDto fileUploadDto)
        {
            var tagNames = JsonConvert.DeserializeObject<IEnumerable<string>>(fileUploadDto.Tags);
            var user = User.GetUserId();
            var fileUploadCommand = FileUploadCommand.Create(fileUploadDto.File, tagNames, user, fileUploadDto.IsPrivate);
            await _mediator.Send(fileUploadCommand);

            return StatusCode(201);
        }
    }
}
