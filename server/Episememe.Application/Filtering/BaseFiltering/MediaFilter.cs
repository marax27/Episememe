using Episememe.Application.Graphs.Interfaces;
using Episememe.Application.Interfaces;
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

        private readonly IGraph<Tag> _tagGraph;

        public MediaFilter(IEnumerable<string>? includedTags, IEnumerable<string>? excludedTags,
            DateTime? timeRangeStart, DateTime? timeRangeEnd, IGraph<Tag> tagGraph)
        {
            _includedTags = includedTags;
            _excludedTags = excludedTags;
            _timeRangeStart = timeRangeStart;
            _timeRangeEnd = timeRangeEnd;
            _tagGraph = tagGraph;
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
                if (_includedTags.Except(_tagGraph.Vertices.Select(t => t.Entity.Name)).Any())
                    return new List<MediaInstance>().AsReadOnly();

                filteredMedia = mediaInstances
                    .Where(mi =>
                        _includedTags.All(it =>
                            mi.MediaTags.Any(mt =>
                                    it == mt.Tag.Name || _tagGraph[it].Successors.Contains(mt.Tag)
                                )
                            )
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
                var relevantExcludedTags = _excludedTags.Where(et => _tagGraph.Vertices.Any(t => t.Entity.Name == et));

                filteredMedia = mediaInstances
                    .Where(mi =>
                        relevantExcludedTags.All(et =>
                            !mi.MediaTags.Any(mt =>
                                    et == mt.Tag.Name || _tagGraph[et].Successors.Contains(mt.Tag)
                                )
                            )
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
