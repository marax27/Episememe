using MediatR;
using Episememe.Application.DataTransfer;
using System.Collections.Generic;

namespace Episememe.Application.Features.GetTags
{
    public class GetTagsQuery : IRequest<IEnumerable<TagInstanceDto>>
    {
        private GetTagsQuery(){}
    
        public static GetTagsQuery Create()
        {
            return new GetTagsQuery();
        }
    }
}
