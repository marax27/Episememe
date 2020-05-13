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
using System;

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
        [Route("revision/{id}")]
        public async Task UpdateTagsList(string id, [FromForm] string tags)
        {
            IEnumerable<string> _tags = tags.Split(",");
            var command = UpdateTagsCommand.Create(id, _tags);
            await _mediator.Send(command);
        }
    }
}
