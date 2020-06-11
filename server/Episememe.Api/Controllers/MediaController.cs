using Episememe.Api.Utilities;
using Episememe.Application.DataTransfer;
using Episememe.Application.Features.MediaRevisionHistory;
using Episememe.Application.Features.SearchMedia;
using Episememe.Application.Features.UpdateTags;
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
            var searchMediaData = new SearchMediaData()
            {
                IncludedTags = searchMediaDto.IncludedTags,
                ExcludedTags = searchMediaDto.ExcludedTags,
                TimeRangeStart = searchMediaDto.TimeRangeStart,
                TimeRangeEnd = searchMediaDto.TimeRangeEnd,
                UserId = User.GetUserId()
            };
            var query = SearchMediaQuery.Create(searchMediaData);
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpPatch]
        [Route("media/{id}")]
        public async Task<IActionResult> UpdateTagsList(string id, [FromBody] TagsUpdateDto listOfTags)
        {
            var command = UpdateTagsCommand.Create(id, listOfTags.Tags, User.GetUserId());
            await _mediator.Send(command);

            return StatusCode(204);
        }

        [HttpGet]
        [Route("media/{id}/history")]
        public async Task<IEnumerable<MediaRevisionHistoryDto>> GetMediaRevisionHistory(string id)
        {
            var query = MediaRevisionHistoryQuery.Create(id, User.GetUserId());
            var result = await _mediator.Send(query);

            return result;
        }
    }
}
