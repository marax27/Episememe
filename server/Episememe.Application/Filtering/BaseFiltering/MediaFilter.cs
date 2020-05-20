using Episememe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Episememe.Application.Filtering.BaseFiltering
{
    public class MediaFilter : IFilter<MediaInstance>
    {
        private readonly IEnumerable<string>? _includedTags;
        private readonly IEnumerable<string>? _excludedTags;
        private readonly DateTime? _timeRangeStart;
        private readonly DateTime? _timeRangeEnd;

        public MediaFilter(IEnumerable<string>? includedTags, IEnumerable<string>? excludedTags,
            DateTime? timeRangeStart, DateTime? timeRangeEnd)
        {
            _includedTags = includedTags;
            _excludedTags = excludedTags;
            _timeRangeStart = timeRangeStart;
            _timeRangeEnd = timeRangeEnd;
        }

        public ReadOnlyCollection<MediaInstance> Filter(ReadOnlyCollection<MediaInstance> instances)
        {
            return InTimeRange(ExcludeTags(IncludeTags(instances)));
        }

        private ReadOnlyCollection<MediaInstance> IncludeTags(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (_includedTags != null)
            {

                filteredMedia = mediaInstances
                    .Where(mi =>
                        !_includedTags.Except(mi.MediaTags.Select(mt => mt.Tag.Name)).Any()
                        )
                    .ToList()
                    .AsReadOnly();
            }

            return filteredMedia;
        }

        private ReadOnlyCollection<MediaInstance> ExcludeTags(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (_excludedTags != null)
            {
                filteredMedia = mediaInstances
                    .Where(mi =>
                        !_excludedTags.Intersect(mi.MediaTags.Select(mt => mt.Tag.Name)).Any()
                        )
                    .ToList()
                    .AsReadOnly();
            }

            return filteredMedia;
        }

        private ReadOnlyCollection<MediaInstance> InTimeRange(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = (_timeRangeStart, _timeRangeEnd) switch
            {
                (null, null) => mediaInstances,
                (null, _) => mediaInstances.Where(mi => mi.Timestamp <= _timeRangeEnd),
                (_, null) => mediaInstances.Where(mi => mi.Timestamp >= _timeRangeStart),
                (_, _) => mediaInstances.Where(mi => mi.Timestamp >= _timeRangeStart
                                                     && mi.Timestamp <= _timeRangeEnd)
            };

            return filteredMedia.ToList().AsReadOnly();
        }
    }
}
