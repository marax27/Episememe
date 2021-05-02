using Episememe.Application.DataTransfer;
using Episememe.Application.Filtering;
using Episememe.Application.Filtering.BaseFiltering;
using Episememe.Application.Graphs.Interfaces;
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
        private readonly IGraph<Tag> _tagGraph;

        public SearchMediaQueryHandler(IApplicationContext context, IGraph<Tag> tagGraph)
        {
            _context = context;
            _tagGraph = tagGraph;
        }

        protected override IEnumerable<MediaInstanceDto> Handle(SearchMediaQuery request)
        {
            var mediaInstances = _context.MediaInstances
                .Include(x => x.MediaTags)
                .ThenInclude(x => x.Tag)
                .ToList()
                .AsReadOnly();

            var filter = ConstructFilter(request.SearchMediaData, _tagGraph);
            var filteredMedia = filter.Filter(mediaInstances)
                .Select(mi =>
                    new MediaInstanceDto(mi.Id, mi.DataType, mi.MediaTags.Select(mt => mt.Tag.Name))
                );

            return filteredMedia;
        }

        private IFilter<MediaInstance> ConstructFilter(SearchMediaData searchMediaData, IGraph<Tag> tagGraph)
        {
            var tagFilter = new TagFilter(searchMediaData.IncludedTags, searchMediaData.ExcludedTags, tagGraph);
            var timeRangeFilter = new TimeRangeFilter(searchMediaData.TimeRangeStart, searchMediaData.TimeRangeEnd);
            var privateMediaFilter = new PrivateMediaFilter(searchMediaData.UserId);
            var favoriteMediaFilter = new FavoriteMediaFilter(searchMediaData.UserId, searchMediaData.FavoritesOnly);
            var filterChain = new FilterChain<MediaInstance>(tagFilter, timeRangeFilter, privateMediaFilter, favoriteMediaFilter);

            return filterChain;
        }
    }
}
