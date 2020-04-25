using Episememe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Episememe.Application.Features.MediaFiltering
{
    public class MediaFilter
    {
        private IQueryable<MediaInstance> MediaInstances;

        public MediaFilter(IQueryable<MediaInstance> mediaInstances)
        {
            MediaInstances = mediaInstances;
        }

        public MediaFilter IncludeTags(IEnumerable<string> includedTags)
        {
            if (includedTags != null)
            {
                MediaInstances = MediaInstances.Where(mi =>
                    includedTags.All(t => mi.MediaTags.Select(mt => mt.Tag.Name).Contains(t))
                );
            }

            return this;
        }

        public MediaFilter ExcludeTags(IEnumerable<string> excludedTags)
        {
            if (excludedTags != null)
            {
                MediaInstances = MediaInstances.Where(mi =>
                    !excludedTags.Any(t => mi.MediaTags.Select(mt => mt.Tag.Name).Contains(t))
                );
            }

            return this;
        }

        public MediaFilter InTimeRange(DateTime? timeRangeStart, DateTime? timeRangeEnd)
        {
            MediaInstances = (timeRangeStart, timeRangeEnd) switch
            {
                (null, null) => MediaInstances,
                (null, _) => MediaInstances.Where(mi => mi.Timestamp <= timeRangeEnd),
                (_, null) => MediaInstances.Where(mi => mi.Timestamp >= timeRangeStart),
                (_, _) => MediaInstances.Where(mi => mi.Timestamp >= timeRangeStart && mi.Timestamp <= timeRangeEnd)
            };

            return this;
        }

        public IQueryable<MediaInstance> Result()
        {
            return MediaInstances;
        }
    }
}
