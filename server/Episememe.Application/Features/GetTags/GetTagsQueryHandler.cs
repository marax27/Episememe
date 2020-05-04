using Episememe.Application.DataTransfer;
using Episememe.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Episememe.Application.Filtering.BaseFiltering;

namespace Episememe.Application.Features.GetTags
{
    public class GetTagsQueryHandler : RequestHandler<GetTagsQuery, IEnumerable<TagInstanceDto>>
    {
        private readonly IApplicationContext _context;

        public GetTagsQueryHandler(IApplicationContext context)
            => _context = context;

        protected override IEnumerable<TagInstanceDto> Handle(GetTagsQuery request)
        {
            var Tags = _context.Tags.Select(mi => new TagInstanceDto(mi.Name, mi.Description));
            return Tags;
        }
    }
}