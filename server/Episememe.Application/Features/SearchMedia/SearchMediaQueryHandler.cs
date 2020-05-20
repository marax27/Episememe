using Episememe.Application.DataTransfer;
using Episememe.Application.Filtering;
using Episememe.Application.Filtering.BaseFiltering;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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

            var filter = ConstructFilter(request.SearchMediaData);
            var filteredMedia = filter.Filter(mediaInstances.ToList().AsReadOnly())
                .Select(mi =>
                    new MediaInstanceDto(mi.Id, mi.DataType, mi.MediaTags.Select(mt => mt.Tag.Name))
                );

            return filteredMedia;
        }

        private IFilter<MediaInstance> ConstructFilter(SearchMediaData searchMediaData)
        {
            var mediaFilter = new MediaFilter(searchMediaData.IncludedTags, searchMediaData.ExcludedTags,
                searchMediaData.TimeRangeStart, searchMediaData.TimeRangeEnd);
            var privateMediaFilter = new PrivateMediaFilter(searchMediaData.UserId);
            var filterChain = new FilterChain<MediaInstance>(mediaFilter, privateMediaFilter);

            return filterChain;
        }
    }
}
