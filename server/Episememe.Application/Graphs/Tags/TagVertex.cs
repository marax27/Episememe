﻿using System.Collections.Generic;
using System.Linq;
using Episememe.Application.Graphs.Exceptions;
using Episememe.Application.Graphs.Interfaces;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;

namespace Episememe.Application.Graphs.Tags
{
    public class TagVertex : IVertex<Tag>
    {
        private readonly IWritableApplicationContext _context;

        public TagVertex(Tag tag, IWritableApplicationContext context)
        {
            Entity = tag;
            _context = context;
        }

        public Tag Entity { get; }

        public IEnumerable<Tag> Successors
            => _context.TagConnections
                .Where(tc => tc.AncestorId == Entity.Id)
                .Select(tc => _context.Tags.Single(x => x.Id == tc.SuccessorId))
                .Distinct();

        public IEnumerable<Tag> Ancestors
            => _context.TagConnections
                .Where(tc => tc.SuccessorId == Entity.Id)
                .Select(tc => _context.Tags.Single(x => x.Id == tc.AncestorId))
                .Distinct();

        public IEnumerable<Tag> Children
            => _context.TagConnections
                .Where(tc => tc.AncestorId == Entity.Id && tc.Hops == 0)
                .Select(tc => _context.Tags.Single(x => x.Id == tc.SuccessorId))
                .Distinct();

        public IEnumerable<Tag> Parents
            => _context.TagConnections
                .Where(tc => tc.SuccessorId == Entity.Id && tc.Hops == 0)
                .Select(tc => _context.Tags.Single(x => x.Id == tc.AncestorId))
                .Distinct();

        public void AddParent(IVertex<Tag> newParent)
        {
            var parentTag = newParent.Entity;
            if (GetDirectConnection(Entity, parentTag) != null)
                return;

            if (Entity == parentTag)
                throw new CycleException($"Loop on element (id: {Entity.Id}) would create a cycle.");

            if (WillConnectionCreateCycle(Entity.Id, parentTag.Id))
                throw new CycleException($"Connection (id: {Entity.Id}) -> (id: {parentTag.Id}) would create a cycle.");

            EnsureTransactionIsOpen();
            ConnectTags(Entity, newParent.Entity);
        }

        public void DeleteParent(IVertex<Tag> parent)
        {
            var connectionToRemove = GetDirectConnection(Entity, parent.Entity);
            if (connectionToRemove != null)
            {
                EnsureTransactionIsOpen();
                DeleteEdge(connectionToRemove);
            }
        }

        private void ConnectTags(Tag child, Tag parent)
        {
            // 1. Direct edge.
            var newDirectEdge = new TagConnection
            {
                SuccessorId = child.Id,
                AncestorId = parent.Id,
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
                    .Where(tc => tc.AncestorId == child.Id)
                    .Select(tc => new TagConnection
                    {
                        EntryEdgeId = tc.Id,
                        DirectEdgeId = newDirectEdgeId,
                        ExitEdgeId = newDirectEdgeId,
                        SuccessorId = tc.SuccessorId,
                        AncestorId = parent.Id,
                        Hops = tc.Hops + 1
                    })
            );

            // 3. A to B's outgoing edges.
            _context.TagConnections.AddRange(
                _context.TagConnections
                    .Where(tc => tc.SuccessorId == parent.Id)
                    .Select(tc => new TagConnection
                    {
                        EntryEdgeId = newDirectEdgeId,
                        DirectEdgeId = newDirectEdgeId,
                        ExitEdgeId = tc.Id,
                        SuccessorId = child.Id,
                        AncestorId = tc.AncestorId,
                        Hops = tc.Hops + 1
                    })
            );

            // 4. A's incoming edges to end vertex of B's outgoing edges.
            var joinResult =
                from start in _context.TagConnections
                from end in _context.TagConnections
                where start.AncestorId == child.Id
                      && end.SuccessorId == parent.Id
                select new { A = start, B = end };

            _context.TagConnections.AddRange(
                joinResult.Select(x => new TagConnection
                {
                    EntryEdgeId = x.A.Id,
                    DirectEdgeId = newDirectEdgeId,
                    ExitEdgeId = x.B.Id,
                    SuccessorId = x.A.SuccessorId,
                    AncestorId = x.B.AncestorId,
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
                    tc.SuccessorId == child.Id
                    && tc.AncestorId == parent.Id
                    && tc.Hops == 0);

        private bool WillConnectionCreateCycle(int childId, int parentId)
            => _context.TagConnections
                .Any(tc => tc.SuccessorId == parentId && tc.AncestorId == childId);

        private void EnsureTransactionIsOpen()
        {
            if (_context.Database.CurrentTransaction == null)
                _context.Database.BeginTransaction();
        }
    }
}
