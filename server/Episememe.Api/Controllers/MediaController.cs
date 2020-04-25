using Episememe.Application.DataTransfer;
using Episememe.Application.Features.SearchMedia;
using Episememe.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IEnumerable<MediaInstanceDto>> GetSearchedMedia([FromBody] SearchMediaDto searchMediaDto)
        {
            var query = SearchMediaQuery.Create(searchMediaDto);
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
