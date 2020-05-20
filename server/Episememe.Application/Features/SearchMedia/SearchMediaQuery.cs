using System;
using Episememe.Application.DataTransfer;
using MediatR;
using System.Collections.Generic;

namespace Episememe.Application.Features.SearchMedia
{
    public class SearchMediaQuery : IRequest<IEnumerable<MediaInstanceDto>>
    {
        public SearchMediaData SearchMediaData { get; }

        private SearchMediaQuery(SearchMediaData searchMediaData)
        {
            SearchMediaData = searchMediaData;
        }

        public static SearchMediaQuery Create(SearchMediaData searchMediaData)
        {
            if(searchMediaData == null)
                throw new ArgumentNullException(nameof(searchMediaData));

            return new SearchMediaQuery(searchMediaData);
        }
    }
}
