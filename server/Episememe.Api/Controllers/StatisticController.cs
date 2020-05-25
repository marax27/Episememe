using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Episememe.Application.Features.GetStatistic;
using Episememe.Application.DataTransfer;

namespace Episememe.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("statistics")]
        public async Task<GetStatisticsDto> GetStatistic()
        {
            var query = GetStatisticsQuery.Create();
            var result = await _mediator.Send(query);
            return result;
        }
    }
}