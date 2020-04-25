using Episememe.Application.DataTransfer;
using Episememe.Application.Features.MediaFiltering;
using Episememe.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Episememe.Application.Features.SearchMedia
{
    public class SearchMediaQueryHandler : RequestHandler<SearchMediaQuery, IEnumerable<MediaInstanceDto>>
    {
        private IApplicationContext _context;

        public SearchMediaQueryHandler(IApplicationContext context)
            => _context = context;

        protected override IEnumerable<MediaInstanceDto> Handle(SearchMediaQuery request)
        {
            var filteredMedia = new MediaFilter(request.SearchMedia)
                .Filter(_context.MediaInstances)
                .Select(mi =>
                    new MediaInstanceDto(mi.Id, mi.MediaTags.Select(mt => mt.Tag.Name))
                );

            return filteredMedia;
        }
    }
}
