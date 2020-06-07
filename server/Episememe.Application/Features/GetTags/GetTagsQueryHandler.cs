using Episememe.Application.DataTransfer;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using Episememe.Application.Graphs.Interfaces;
using Episememe.Domain.Entities;

namespace Episememe.Application.Features.GetTags
{
    public class GetTagsQueryHandler : RequestHandler<GetTagsQuery, IEnumerable<TagInstanceDto>>
    {
        private readonly IGraph<Tag> _graph;

        public GetTagsQueryHandler(IGraph<Tag> graph)
            => _graph = graph;

        protected override IEnumerable<TagInstanceDto> Handle(GetTagsQuery request)
        {
            return _graph.Vertices.Select(vertex => new TagInstanceDto(
                vertex.Entity.Name,
                vertex.Entity.Description,
                vertex.Successors.Select(tag => tag.Name),
                vertex.Ancestors.Select(tag => tag.Name)
            ));
        }
    }
}