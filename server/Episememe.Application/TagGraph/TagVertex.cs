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

        public void AddParent(TagVertex newParent)
        {
            ConnectTags(_tag.Id, newParent._tag.Id);
        }

        public void DeleteParent(TagVertex parent)
        {
            DisconnectTags(_tag.Id, parent._tag.Id);
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

        private void ConnectTags(int childId, int parentId)
        {
            // Direct connection exists.
            if (_context.TagConnections
                .Any(tc => tc.Successor == childId && tc.Ancestor == parentId && tc.Hops == 0))
                return;

            if (childId == parentId)
                return;

            // Check for circular references?
            if (_context.TagConnections
                .Any(tc => tc.Successor == parentId && tc.Ancestor == childId))
                return;

            // Direct edge.
            var newDirectEdge = new TagConnection
            {
                Successor = childId,
                Ancestor = parentId,
                Hops = 0
            };
            _context.TagConnections.Add(newDirectEdge);
            _context.SaveChanges();

            var newDirectEdgeId = newDirectEdge.Id;
            newDirectEdge.EntryEdgeId = newDirectEdgeId;
            newDirectEdge.ExitEdgeId = newDirectEdgeId;
            newDirectEdge.DirectEdgeId = newDirectEdgeId;

            // (quote) Step 1: A's incoming edges to B.
            _context.TagConnections.AddRange(
                _context.TagConnections
                    .Where(tc => tc.Ancestor == childId)
                    .Select(tc => new TagConnection
                    {
                        EntryEdgeId = tc.Id,
                        DirectEdgeId = newDirectEdgeId,
                        ExitEdgeId = newDirectEdgeId,
                        Successor = tc.Successor,
                        Ancestor = parentId,
                        Hops = tc.Hops + 1
                    })
            );

            // (quote) Step 2: A to B's outgoing edges.
            _context.TagConnections.AddRange(
                _context.TagConnections
                    .Where(tc => tc.Successor == parentId)
                    .Select(tc => new TagConnection
                    {
                        EntryEdgeId = newDirectEdgeId,
                        DirectEdgeId = newDirectEdgeId,
                        ExitEdgeId = tc.Id,
                        Successor = childId,
                        Ancestor = tc.Ancestor,
                        Hops = tc.Hops + 1
                    })
            );

            // (quote) Step 3: A's incoming edges to end vertex of B's outgoing edges.
            var joinResult =
                from start in _context.TagConnections
                from end in _context.TagConnections
                where start.Ancestor == childId
                      && end.Successor == parentId
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

        private void DisconnectTags(int child, int parent)
        {
            var directEdge = _context.TagConnections
                .SingleOrDefault(tc =>
                    tc.Successor == child
                    && tc.Ancestor == parent
                    && tc.Hops == 0);

            if (directEdge == null)
                return;
            DeleteEdge(directEdge.Id);
        }

        private void DeleteEdge(int edgeId)
        {
            if (_context.TagConnections.Find(edgeId) == null)
                return;

            var purgeList = new List<TagConnection>();

            // (quote) 1. Rows originally inserted with the 1st AddEdge call, for this direct edge.
            purgeList.AddRange(_context.TagConnections
                .Where(tc => tc.DirectEdgeId == edgeId));

            // (quote) 2. All dependent rows inserted afterwards.
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

            _context.TagConnections.RemoveRange(purgeList);
        }
    }
}
