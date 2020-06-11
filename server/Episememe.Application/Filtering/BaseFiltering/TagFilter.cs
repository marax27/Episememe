using Episememe.Application.Graphs.Interfaces;
using Episememe.Domain.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Episememe.Application.Filtering.BaseFiltering
{
    public class TagFilter : IFilter<MediaInstance>
    {
        private readonly IEnumerable<string>? _includedTags;
        private readonly IEnumerable<string>? _excludedTags;

        private readonly IGraph<Tag> _tagGraph;

        public TagFilter(IEnumerable<string>? includedTags, IEnumerable<string>? excludedTags, IGraph<Tag> tagGraph)
        {
            _includedTags = includedTags;
            _excludedTags = excludedTags;
            _tagGraph = tagGraph;
        }

        public ReadOnlyCollection<MediaInstance> Filter(ReadOnlyCollection<MediaInstance> instances)
        {
            return ExcludeTags(IncludeTags(instances));
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
    }
}
