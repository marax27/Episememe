using Episememe.Application.DataTransfer;
using Episememe.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return new SearchMediaQuery(searchMedia);
        }
    }
}
