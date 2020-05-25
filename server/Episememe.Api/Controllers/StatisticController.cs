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
    public class StatisticController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("statistic")]
        public async Task<GetStatisticsDto> GetStatistic()
        {
            var query = GetStatisticQuery.Create();
            var result = await _mediator.Send(query);
            return result;
        }
    }
}