using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Episememe.Application.DataTransfer;
using Episememe.Application.Filtering.BaseFiltering;
using Episememe.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                .ToList()
                .AsReadOnly();

            var filter = new FavoriteMediaFilter(request.UserId);

            var filteredMedia = filter.Filter(mediaInstances)
                .Select(mi =>
                    new MediaInstanceDto(mi.Id, mi.DataType, mi.MediaTags.Select(mt => mt.Tag.Name))
                );

            return filteredMedia;
        }
    }
}
