using System.Collections.Generic;
using System.Linq;
using Episememe.Application.Common;
using Episememe.Application.TagGraph.Exceptions;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;

namespace Episememe.Application.TagGraph
{
    public class TagVertex
    {
        private readonly Tag _tag;

        public TagVertex(Tag tag)
            => _tag = tag;

        public void AddParent(TagVertex parent)
        {
            if (AddingParentCreatesCycle(parent))
            {
                throw new CycleException($"Making '{parent._tag.Name}' a parent of '{_tag.Name}' would create a cycle.");
            }

            var directConnection = new TagConnection
            {
                Successor = _tag,
                Ancestor = parent._tag,
                Depth = 1
            };

            var indirectConnections = parent._tag.Ancestors
                .Select(conn => new TagConnection
                {
                    Successor = _tag,
                    Ancestor = conn.Ancestor,
                    Depth = conn.Depth + 1
                }).ToList();

            _tag.Ancestors.Add(directConnection);
            _tag.Ancestors.AddRange(indirectConnections);
        }

        public void AddChild(TagVertex child)
        {
            child.AddParent(this);
        }

        public IEnumerable<Tag> AncestorTags
            => _tag.Ancestors.Select(tc => tc.Ancestor)
                .Distinct();

        public IEnumerable<Tag> SuccessorTags
            => _tag.Successors.Select(tc => tc.Successor)
                .Distinct();

        private bool AddingParentCreatesCycle(TagVertex potentialParent)
        {
            var ancestorIds = potentialParent._tag.Ancestors
                .Select(tc => tc.AncestorId)
                .Concat(new[] { potentialParent._tag.Id });
            var currentSuccessorIds = _tag.Successors
                .Select(tc => tc.SuccessorId);

            return ancestorIds.Intersect(currentSuccessorIds).Any();
        }
    }
}
