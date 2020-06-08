using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Episememe.Application.Features.GetTags;
using Episememe.Application.DataTransfer;
using Episememe.Application.Features.SetTag;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class TagsController : ControllerBase
    {
        private readonly ILogger<TagsController> _logger;
        private readonly IMediator _mediator;

        public TagsController(ILogger<TagsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("tags")]
        public async Task<IEnumerable<TagInstanceDto>> GetTags()
        {
            var query = GetTagsQuery.Create();
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpPut]
        [Route("tags/{name}")]
        public async Task<NoContentResult> SetTag(string name, [FromBody] SetTagDto setTagDto)
        {
            var command = SetTagCommand.Create(name, setTagDto);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}