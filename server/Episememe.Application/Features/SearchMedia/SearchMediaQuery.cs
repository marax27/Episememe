using System;
using Episememe.Application.DataTransfer;
using MediatR;
using System.Collections.Generic;

namespace Episememe.Application.Features.SearchMedia
{
    public class SearchMediaQuery : IRequest<IEnumerable<MediaInstanceDto>>
    {
        public SearchMediaDto SearchMedia { get; }

        private SearchMediaQuery(SearchMediaDto searchMedia)
        {
            SearchMedia = searchMedia;
        }

        public static SearchMediaQuery Create(SearchMediaDto searchMedia)
        {
            if (searchMedia == null)
                throw new ArgumentNullException();
            
            return new SearchMediaQuery(searchMedia);
        }
    }
}
