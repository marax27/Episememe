using MediatR;
using Episememe.Application.DataTransfer;

namespace Episememe.Application.Features.GetStatistics
{
    public class GetStatisticsQuery : IRequest<MediaTimeDto>
    {
        private GetStatisticsQuery(){}
    
        public static GetStatisticsQuery Create()
        {
            return new GetStatisticsQuery();
        }
    }
}