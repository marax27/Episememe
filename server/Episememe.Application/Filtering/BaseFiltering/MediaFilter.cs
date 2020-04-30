using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Episememe.Application.DataTransfer;
using Episememe.Domain.Entities;

namespace Episememe.Application.Filtering.BaseFiltering
{
    public class MediaFilter
    {
        private readonly SearchMediaDto _searchMedia;

        public MediaFilter(SearchMediaDto searchMedia)
        {
            _searchMedia = searchMedia;
        }

        public IEnumerable<MediaInstance> Filter(IQueryable<MediaInstance> mediaInstances)
        {
            ReadOnlyCollection<MediaInstance> filteredMedia = mediaInstances.ToList().AsReadOnly();
            filteredMedia = InTimeRange(ExcludeTags((IncludeTags(filteredMedia))));
            
            return filteredMedia.AsEnumerable();
        }

        private ReadOnlyCollection<MediaInstance> IncludeTags(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (_searchMedia.IncludedTags != null)
            {
                
                filteredMedia = mediaInstances
                    .Where(mi => 
                        !_searchMedia.IncludedTags.Except(mi.MediaTags.Select(mt => mt.Tag.Name)).Any()
                        )
                    .ToList()
                    .AsReadOnly();
            }

            return filteredMedia;
        }

        private ReadOnlyCollection<MediaInstance> ExcludeTags(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (_searchMedia.ExcludedTags != null)
            {
                filteredMedia = mediaInstances
                    .Where(mi => 
                        !_searchMedia.ExcludedTags.Intersect(mi.MediaTags.Select(mt => mt.Tag.Name)).Any()
                        )
                    .ToList()
                    .AsReadOnly();
            }

            return filteredMedia;
        }

        private ReadOnlyCollection<MediaInstance> InTimeRange(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = (_searchMedia.TimeRangeStart, _searchMedia.TimeRangeEnd) switch
            {
                (null, null) => mediaInstances,
                (null, _) => mediaInstances.Where(mi => mi.Timestamp <= _searchMedia.TimeRangeEnd),
                (_, null) => mediaInstances.Where(mi => mi.Timestamp >= _searchMedia.TimeRangeStart),
                (_, _) => mediaInstances.Where(mi => mi.Timestamp >= _searchMedia.TimeRangeStart && mi.Timestamp <= _searchMedia.TimeRangeEnd)
            };

            return filteredMedia.ToList().AsReadOnly();
        }
    }
}
