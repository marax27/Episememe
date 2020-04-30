using Episememe.Application.DataTransfer;
using Episememe.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Episememe.Application.Filtering.BaseFiltering;

namespace Episememe.Application.Features.SearchMedia
{
    public class SearchMediaQueryHandler : RequestHandler<SearchMediaQuery, IEnumerable<MediaInstanceDto>>
    {
        private readonly IApplicationContext _context;

        public SearchMediaQueryHandler(IApplicationContext context)
            => _context = context;

        protected override IEnumerable<MediaInstanceDto> Handle(SearchMediaQuery request)
        {
            var mediaInstances = _context.MediaInstances
                .Include(x => x.MediaTags)
                .ThenInclude(x => x.Tag);
            var filteredMedia = new MediaFilter(request.SearchMedia)
                .Filter(mediaInstances)
                .Select(mi =>
                    new MediaInstanceDto(mi.Id, mi.DataType, mi.MediaTags.Select(mt => mt.Tag.Name))
                );

            return filteredMedia;
        }
    }
}
