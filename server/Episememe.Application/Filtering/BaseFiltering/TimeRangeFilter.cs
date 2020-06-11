using Episememe.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Episememe.Application.Filtering.BaseFiltering
{
    public class TimeRangeFilter : IFilter<MediaInstance>
    {
        private readonly DateTime? _timeRangeStart;
        private readonly DateTime? _timeRangeEnd;

        public TimeRangeFilter(DateTime? timeRangeStart, DateTime? timeRangeEnd)
        {
            _timeRangeStart = timeRangeStart;
            _timeRangeEnd = timeRangeEnd;
        }

        public ReadOnlyCollection<MediaInstance> Filter(ReadOnlyCollection<MediaInstance> instances)
        {
            return InTimeRange(instances);
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
