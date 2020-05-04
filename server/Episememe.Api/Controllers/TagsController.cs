using System.Threading.Tasks;
using Episememe.Api.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Episememe.Application.Features.GetTags;
using Episememe.Application.DataTransfer;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class TagsController : ControllerBase
    {
        private readonly ILogger<TagController> _logger;
        private readonly IMediator _mediator;

        public TagController(ILogger<TagController> logger, IMediator mediator)
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
    }
}