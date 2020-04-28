using Episememe.Application.DataTransfer;
using Episememe.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Episememe.Application.Features.MediaFiltering
{
    public class MediaFilter
    {
        private SearchMediaDto SearchMedia { get; }

        public MediaFilter(SearchMediaDto searchMedia)
        {
            SearchMedia = searchMedia;
        }

        public IEnumerable<MediaInstance> Filter(IQueryable<MediaInstance> mediaInstances)
        {
            IEnumerable<MediaInstance> filteredMedia = mediaInstances.ToList();
            filteredMedia = IncludeTags(ExcludeTags(InTimeRange(filteredMedia)));

            return filteredMedia;
        }

        private IEnumerable<MediaInstance> IncludeTags(IEnumerable<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (SearchMedia.IncludedTags != null)
            {
                filteredMedia = mediaInstances.Where(mi =>
                    !SearchMedia.IncludedTags.Except(mi.MediaTags.Select(mt => mt.Tag.Name)).Any()
                );
            }

            return filteredMedia;
        }

        private IEnumerable<MediaInstance> ExcludeTags(IEnumerable<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (SearchMedia.ExcludedTags != null)
            {
                filteredMedia = mediaInstances.Where(mi =>
                    !SearchMedia.ExcludedTags.Intersect(mi.MediaTags.Select(mt => mt.Tag.Name)).Any()
                );
            }

            return filteredMedia;
        }

        private IEnumerable<MediaInstance> InTimeRange(IEnumerable<MediaInstance> mediaInstances)
        {
            var filteredMedia = (SearchMedia.TimeRangeStart, SearchMedia.TimeRangeEnd) switch
            {
                (null, null) => mediaInstances,
                (null, _) => mediaInstances.Where(mi => mi.Timestamp <= SearchMedia.TimeRangeEnd),
                (_, null) => mediaInstances.Where(mi => mi.Timestamp >= SearchMedia.TimeRangeStart),
                (_, _) => mediaInstances.Where(mi => mi.Timestamp >= SearchMedia.TimeRangeStart && mi.Timestamp <= SearchMedia.TimeRangeEnd)
            };

            return filteredMedia;
        }
    }
}
