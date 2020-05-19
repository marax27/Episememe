using Episememe.Application.Features.SearchMedia;
using Episememe.Domain.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Episememe.Application.Filtering.BaseFiltering
{
    public class MediaFilter
    {
        private readonly SearchMediaData _searchMediaData;

        public MediaFilter(SearchMediaData searchMediaData)
        {
            _searchMediaData = searchMediaData;
        }

        public IEnumerable<MediaInstance> Filter(IQueryable<MediaInstance> mediaInstances)
        {
            ReadOnlyCollection<MediaInstance> filteredMedia = mediaInstances.ToList().AsReadOnly();
            filteredMedia = ConsiderPrivate(InTimeRange(ExcludeTags(IncludeTags(filteredMedia))));

            return filteredMedia.AsEnumerable();
        }

        private ReadOnlyCollection<MediaInstance> IncludeTags(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (_searchMediaData.IncludedTags != null)
            {

                filteredMedia = mediaInstances
                    .Where(mi =>
                        !_searchMediaData.IncludedTags.Except(mi.MediaTags.Select(mt => mt.Tag.Name)).Any()
                        )
                    .ToList()
                    .AsReadOnly();
            }

            return filteredMedia;
        }

        private ReadOnlyCollection<MediaInstance> ExcludeTags(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (_searchMediaData.ExcludedTags != null)
            {
                filteredMedia = mediaInstances
                    .Where(mi =>
                        !_searchMediaData.ExcludedTags.Intersect(mi.MediaTags.Select(mt => mt.Tag.Name)).Any()
                        )
                    .ToList()
                    .AsReadOnly();
            }

            return filteredMedia;
        }

        private ReadOnlyCollection<MediaInstance> InTimeRange(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = (_searchMediaData.TimeRangeStart, _searchMediaData.TimeRangeEnd) switch
            {
                (null, null) => mediaInstances,
                (null, _) => mediaInstances.Where(mi => mi.Timestamp <= _searchMediaData.TimeRangeEnd),
                (_, null) => mediaInstances.Where(mi => mi.Timestamp >= _searchMediaData.TimeRangeStart),
                (_, _) => mediaInstances.Where(mi => mi.Timestamp >= _searchMediaData.TimeRangeStart 
                                                     && mi.Timestamp <= _searchMediaData.TimeRangeEnd)
            };

            return filteredMedia.ToList().AsReadOnly();
        }

        private ReadOnlyCollection<MediaInstance> ConsiderPrivate(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (!string.IsNullOrEmpty(_searchMediaData.UserId))
            {
                filteredMedia = mediaInstances.Where(mi => !mi.IsPrivate 
                                                           || (mi.IsPrivate && mi.AuthorId == _searchMediaData.UserId))
                    .ToList()
                    .AsReadOnly();
            }

            return filteredMedia;
        }
    }
}
