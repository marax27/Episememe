using Episememe.Application.DataTransfer;
using Episememe.Application.Filtering.BaseFiltering;
using Episememe.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Episememe.Application.Features.GetFavoriteMedia
{
    public class GetFavoriteMediaQueryHandler : RequestHandler<GetFavoriteMediaQuery, IEnumerable<MediaInstanceDto>>
    {
        private readonly IApplicationContext _context;

        public GetFavoriteMediaQueryHandler(IApplicationContext context)
        {
            _context = context;
        }

        protected override IEnumerable<MediaInstanceDto> Handle(GetFavoriteMediaQuery request)
        {
            var mediaInstances = _context.MediaInstances
                .Include(mi => mi.FavoriteMedia)
                .Include(x => x.MediaTags)
                .ThenInclude(x => x.Tag)
                .ToList()
                .AsReadOnly();

            var filter = new FavoriteMediaFilter(request.UserId, true);

            var filteredMedia = filter.Filter(mediaInstances)
                .Select(mi =>
                    new MediaInstanceDto(mi.Id, mi.DataType, mi.MediaTags.Select(mt => mt.Tag.Name))
                );

            return filteredMedia;
        }
    }
}
