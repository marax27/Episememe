using System.Threading.Tasks;
using Episememe.Application.Features.GenerateBrowseToken;
using Episememe.Application.Features.IssueBrowseToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api/authorization")]
    [Authorize]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<string> Post()
        {
            var query = GenerateBrowseTokenQuery.Create();
            var browseToken = await _mediator.Send(query);

            var command = IssueBrowseTokenCommand.Create(browseToken);
            await _mediator.Send(command);
            return browseToken;
        }
    }
}
