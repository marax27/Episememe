using System.Collections.Generic;
using System.Threading.Tasks;
using Episememe.Api.Utilities;
using Episememe.Application.DataTransfer;
using Episememe.Application.Features.GetFavoriteMedia;
using Episememe.Application.Features.MarkFavoriteMedia;
using Episememe.Application.Features.RemoveFavoriteMedia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class FavoriteMediaController : ControllerBase
    {
        private readonly ILogger<MediaController> _logger;
        private readonly IMediator _mediator;

        public FavoriteMediaController(ILogger<MediaController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("favorite")]
        public async Task<IEnumerable<MediaInstanceDto>> GetFavoriteMedia()
        {
            var query = GetFavoriteMediaQuery.Create(User.GetUserId());
            var favoriteMediaInstances = await _mediator.Send(query);

            return favoriteMediaInstances;
        }

        [HttpPost]
        [Route("favorite/{id}")]
        public async Task<IActionResult> MarkMediaAsFavorite(string id)
        {
            var command = MarkFavoriteMediaCommand.Create(id, User.GetUserId());
            await _mediator.Send(command);

            return StatusCode(200);
        }

        [HttpDelete]
        [Route("favorite/{id}")]
        public async Task<IActionResult> RemoveMediaFromFavorites(string id)
        {
            var command = RemoveFavoriteMediaCommand.Create(id, User.GetUserId());
            await _mediator.Send(command);

            return StatusCode(200);
        }
    }
}
