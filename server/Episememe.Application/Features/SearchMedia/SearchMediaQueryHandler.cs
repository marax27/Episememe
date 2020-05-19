using Episememe.Application.DataTransfer;
using Episememe.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Episememe.Application.Filtering;
using Episememe.Application.Filtering.BaseFiltering;
using Episememe.Domain.Entities;

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
            var mediaFilter = new MediaFilter(request.SearchMediaData.IncludedTags, request.SearchMediaData.ExcludedTags,
                request.SearchMediaData.TimeRangeStart, request.SearchMediaData.TimeRangeEnd);
            var privateMediaFilter = new PrivateMediaFilter(request.SearchMediaData.UserId);
            var filterChain = new FilterChain<MediaInstance>(mediaFilter, privateMediaFilter);

            var filteredMedia = filterChain.Filter(mediaInstances.ToList().AsReadOnly())
                .Select(mi =>
                    new MediaInstanceDto(mi.Id, mi.DataType, mi.MediaTags.Select(mt => mt.Tag.Name))
                );

            return filteredMedia;
        }
    }
}
