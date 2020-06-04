using System.Collections.Generic;
using System.Linq;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;

namespace Episememe.Application.TagGraph
{
    public class TagVertex
    {
        private readonly Tag _tag;
        private readonly IWritableApplicationContext _context;

        public TagVertex(Tag tag, IWritableApplicationContext context)
        {
            _tag = tag;
            _context = context;
        }

        public IEnumerable<Tag> Successors
            => _context.TagConnections
                .Where(tc => tc.Ancestor == _tag.Id)
                .Select(tc => _context.Tags.Single(x => x.Id == tc.Successor))
                .Distinct();

        public IEnumerable<Tag> Ancestors
            => _context.TagConnections
                .Where(tc => tc.Successor == _tag.Id)
                .Select(tc => _context.Tags.Single(x => x.Id == tc.Ancestor))
                .Distinct();

        public void AddParent(TagVertex newParent)
        {
            var parentTag = newParent._tag;
            if (GetDirectConnection(_tag, parentTag) != null)
                return;

            if (_tag == parentTag)
                throw new CycleException($"Loop on element (id: {_tag.Id}) would create a cycle.");

            if (WillConnectionCreateCycle(_tag.Id, parentTag.Id))
                throw new CycleException($"Connection (id: {_tag.Id}) -> (id: {parentTag.Id}) would create a cycle.");

            ConnectTags(_tag, newParent._tag);
        }

        public void DeleteParent(TagVertex parent)
        {
            var connectionToRemove = GetDirectConnection(_tag, parent._tag);
            if (connectionToRemove != null)
                DeleteEdge(connectionToRemove);
        }

        private void ConnectTags(Tag child, Tag parent)
        {
            // 1. Direct edge.
            var newDirectEdge = new TagConnection
            {
                Successor = child.Id,
                Ancestor = parent.Id,
                Hops = 0
            };
            _context.TagConnections.Add(newDirectEdge);
            _context.SaveChanges();

            var newDirectEdgeId = newDirectEdge.Id;
            newDirectEdge.EntryEdgeId = newDirectEdgeId;
            newDirectEdge.ExitEdgeId = newDirectEdgeId;
            newDirectEdge.DirectEdgeId = newDirectEdgeId;

            // 2. A's incoming edges to B.
            _context.TagConnections.AddRange(
                _context.TagConnections
                    .Where(tc => tc.Ancestor == child.Id)
                    .Select(tc => new TagConnection
                    {
                        EntryEdgeId = tc.Id,
                        DirectEdgeId = newDirectEdgeId,
                        ExitEdgeId = newDirectEdgeId,
                        Successor = tc.Successor,
                        Ancestor = parent.Id,
                        Hops = tc.Hops + 1
                    })
            );

            // 3. A to B's outgoing edges.
            _context.TagConnections.AddRange(
                _context.TagConnections
                    .Where(tc => tc.Successor == parent.Id)
                    .Select(tc => new TagConnection
                    {
                        EntryEdgeId = newDirectEdgeId,
                        DirectEdgeId = newDirectEdgeId,
                        ExitEdgeId = tc.Id,
                        Successor = child.Id,
                        Ancestor = tc.Ancestor,
                        Hops = tc.Hops + 1
                    })
            );

            // 4. A's incoming edges to end vertex of B's outgoing edges.
            var joinResult =
                from start in _context.TagConnections
                from end in _context.TagConnections
                where start.Ancestor == child.Id
                      && end.Successor == parent.Id
                select new { A = start, B = end };

            _context.TagConnections.AddRange(
                joinResult.Select(x => new TagConnection
                {
                    EntryEdgeId = x.A.Id,
                    DirectEdgeId = newDirectEdgeId,
                    ExitEdgeId = x.B.Id,
                    Successor = x.A.Successor,
                    Ancestor = x.B.Ancestor,
                    Hops = x.A.Hops + x.B.Hops + 1
                })
            );
        }

        private void DeleteEdge(TagConnection edge)
        {
            var purgeList = GetRelatedEdgesToRemove(edge);
            _context.TagConnections.RemoveRange(purgeList);
        }

        private IEnumerable<TagConnection> GetRelatedEdgesToRemove(TagConnection edge)
        {
            var purgeList = new List<TagConnection>();

            // 1. Rows originally inserted with the 1st AddEdge call, for this direct edge.
            purgeList.AddRange(_context.TagConnections
                .Where(tc => tc.DirectEdgeId == edge.Id));

            // 2. All dependent rows inserted afterwards.
            while (true)
            {
                var edgesToRemove = _context.TagConnections
                    .Where(tc => tc.Hops > 0)
                    .AsEnumerable()
                    .Where(tc =>
                        purgeList.Any(x => x.Id == tc.EntryEdgeId || x.Id == tc.ExitEdgeId)
                        && purgeList.All(x => x.Id != tc.Id))
                    .ToList();

                purgeList.AddRange(edgesToRemove);

                if (!edgesToRemove.Any())
                    break;
            }

            return purgeList;
        }

        private TagConnection? GetDirectConnection(Tag child, Tag parent)
            => _context.TagConnections.SingleOrDefault(tc =>
                    tc.Successor == child.Id
                    && tc.Ancestor == parent.Id
                    && tc.Hops == 0);

        private bool WillConnectionCreateCycle(int childId, int parentId)
            => _context.TagConnections
                .Any(tc => tc.Successor == parentId && tc.Ancestor == childId);
    }
}
