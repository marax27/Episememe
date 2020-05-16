using Episememe.Application.DataTransfer;
using Episememe.Application.Features.SearchMedia;
using Episememe.Application.Features.UpdateTags;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class MediaController : ControllerBase
    {
        private readonly ILogger<MediaController> _logger;
        private readonly IMediator _mediator;

        public MediaController(ILogger<MediaController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("media")]
        public async Task<IEnumerable<MediaInstanceDto>> GetSearchedMedia([FromQuery] string q)
        {
            var searchMediaDto = JsonConvert.DeserializeObject<SearchMediaDto>(q);
            var query = SearchMediaQuery.Create(searchMediaDto);
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpPost]
        [Route("revision")]
        public async Task<IActionResult> UpdateTagsList([FromForm] TagsUpdateDto ListOfTags)
        {
            var command = UpdateTagsCommand.Create(ListOfTags.FileId!, ListOfTags.Tags!);
            await _mediator.Send(command);

            return StatusCode(201);
        }
    }
}
