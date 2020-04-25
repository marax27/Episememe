using Episememe.Application.DataTransfer;
using Episememe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Episememe.Application.Features.MediaFiltering
{
    public class MediaFilter
    {
        private SearchMediaDto SearchMedia;

        public MediaFilter(SearchMediaDto searchMedia)
        {
            SearchMedia = searchMedia;
        }

        public IQueryable<MediaInstance> Filter(IQueryable<MediaInstance> mediaInstances)
        {
            IncludeTags(ref mediaInstances)
                .ExcludeTags(ref mediaInstances)
                .InTimeRange(ref mediaInstances);

            return mediaInstances;
        }

        private MediaFilter IncludeTags(ref IQueryable<MediaInstance> mediaInstances)
        {
            if (SearchMedia.IncludedTags != null)
            {
                mediaInstances = mediaInstances.Where(mi =>
                    SearchMedia.IncludedTags.All(t => mi.MediaTags.Select(mt => mt.Tag.Name).Contains(t))
                );
            }

            return this;
        }

        private MediaFilter ExcludeTags(ref IQueryable<MediaInstance> mediaInstances)
        {
            if (SearchMedia.ExcludedTags != null)
            {
                mediaInstances = mediaInstances.Where(mi =>
                    !SearchMedia.ExcludedTags.Any(t => mi.MediaTags.Select(mt => mt.Tag.Name).Contains(t))
                );
            }

            return this;
        }

        private MediaFilter InTimeRange(ref IQueryable<MediaInstance> mediaInstances)
        {
            mediaInstances = (SearchMedia.TimeRangeStart, SearchMedia.TimeRangeEnd) switch
            {
                (null, null) => mediaInstances,
                (null, _) => mediaInstances.Where(mi => mi.Timestamp <= SearchMedia.TimeRangeEnd),
                (_, null) => mediaInstances.Where(mi => mi.Timestamp >= SearchMedia.TimeRangeStart),
                (_, _) => mediaInstances.Where(mi => mi.Timestamp >= SearchMedia.TimeRangeStart && mi.Timestamp <= SearchMedia.TimeRangeEnd)
            };

            return this;
        }
    }
}
