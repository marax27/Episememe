using System.Collections.Generic;
using System.Linq;
using Episememe.Application.Graphs.Interfaces;
using Episememe.Domain.Entities;
using MediatR;

namespace Episememe.Application.Features.SetTag
{
    public class SetTagCommandHandler : RequestHandler<SetTagCommand>
    {
        private readonly IGraph<Tag> _graph;

        public SetTagCommandHandler(IGraph<Tag> graph)
        {
            _graph = graph;
        }

        protected override void Handle(SetTagCommand request)
        {
            var newChildren = request.NewChildren;
            var newParents = request.NewParents;

            CreateNonexistentTags(newChildren.Concat(newParents));

            var vertex = _graph[request.CurrentName];
            UpdateGraphEdges(vertex, newChildren, newParents);
            UpdateTagDetails(vertex, request.NewName, request.NewDescription);

            _graph.CommitAllChanges();
        }

        private void CreateNonexistentTags(IEnumerable<string> tagNames)
        {
            var existingNames = _graph.Vertices.Select(vertex => vertex.Entity.Name);

            foreach (var tagName in tagNames.Except(existingNames))
                _graph.Add(new Tag {Name = tagName});

            _graph.SaveChanges();
        }

        private void UpdateGraphEdges(
            IVertex<Tag> vertex, IReadOnlyCollection<string> newSuccessors, IReadOnlyCollection<string> newAncestors)
        {
            var oldSuccessors = vertex.Successors.Select(tag => tag.Name)
                .ToList().AsReadOnly();
            var oldAncestors = vertex.Ancestors.Select(tag => tag.Name)
                .ToList().AsReadOnly();

            foreach(var tagName in oldAncestors.Except(newAncestors))
                vertex.DeleteParent(_graph[tagName]);
            foreach(var tagName in oldSuccessors.Except(newSuccessors))
                vertex.DeleteChild(_graph[tagName]);
            foreach(var tagName in newAncestors.Except(oldAncestors))
                vertex.AddParent(_graph[tagName]);
            foreach(var tagName in newSuccessors.Except(oldSuccessors))
                vertex.AddChild(_graph[tagName]);
        }

        private void UpdateTagDetails(IVertex<Tag> vertex, string name, string? description)
        {
            var tag = vertex.Entity;
            tag.Name = name;
            tag.Description = description;
        }
    }
}
