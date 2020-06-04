using System.Linq;
using System.Collections.Generic;
using Episememe.Application.Interfaces;
using Episememe.Domain.HelperEntities;

namespace Episememe.Application.TagGraph
{
    public interface ITagGraphService
    {
        void Connect(int child, int parent);
        void Disconnect(int child, int parent);
        void SaveChanges();
        IEnumerable<int> Successors(int id);
        IEnumerable<int> Ancestors(int id);
    }

    public class TagGraphService : ITagGraphService
    {
        private readonly IWritableApplicationContext _context;

        public TagGraphService(IWritableApplicationContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<int> Successors(int id)
        {
            return _context.TagConnections
                .Where(tc => tc.Ancestor == id)
                .Select(tc => tc.Successor);
        }

        public IEnumerable<int> Ancestors(int id)
        {
            return _context.TagConnections
                .Where(tc => tc.Successor == id)
                .Select(tc => tc.Ancestor);
        }

        public void Connect(int child, int parent)
        {
            // Direct connection exists.
            if (_context.TagConnections
                .Any(tc => tc.Successor == child && tc.Ancestor == parent && tc.Hops == 0))
                return;

            if (child == parent)
                return;

            // Check for circular references?
            if (_context.TagConnections
                .Any(tc => tc.Successor == parent && tc.Ancestor == child))
                return;

            // Direct edge.
            var newDirectEdge = new TagConnection
            {
                Successor = child,
                Ancestor = parent,
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
                    .Where(tc => tc.Ancestor == child)
                    .Select(tc => new TagConnection
                    {
                        EntryEdgeId = tc.Id,
                        DirectEdgeId = newDirectEdgeId,
                        ExitEdgeId = newDirectEdgeId,
                        Successor = tc.Successor,
                        Ancestor = parent,
                        Hops = tc.Hops + 1
                    })
            );

            // (quote) Step 2: A to B's outgoing edges.
            _context.TagConnections.AddRange(
                _context.TagConnections
                    .Where(tc => tc.Successor == parent)
                    .Select(tc => new TagConnection
                    {
                        EntryEdgeId = newDirectEdgeId,
                        DirectEdgeId = newDirectEdgeId,
                        ExitEdgeId = tc.Id,
                        Successor = child,
                        Ancestor = tc.Ancestor,
                        Hops = tc.Hops + 1
                    })
            );

            // (quote) Step 3: A's incoming edges to end vertex of B's outgoing edges.
            var joinResult =
                from start in _context.TagConnections
                from end in _context.TagConnections
                where start.Ancestor == child
                      && end.Successor == parent
                select new {A = start, B = end};

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

        public void Disconnect(int child, int parent)
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
