using MediatR;
using Episememe.Application.DataTransfer;

namespace Episememe.Application.Features.GetStatistic
{
    public class GetStatisticQuery : IRequest<GetStatisticsDto>
    {
        private GetStatisticQuery(){}
    
        public static GetStatisticQuery Create()
        {
            return new GetStatisticQuery();
        }
    }
}