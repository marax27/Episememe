using Episememe.Application.DataTransfer;
using Episememe.Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Episememe.Application.Features.MediaRevisionHistory
{
    public class MediaRevisionHistoryQueryHandler : RequestHandler<MediaRevisionHistoryQuery, IEnumerable<MediaRevisionHistoryDto>>
    {
        private readonly IApplicationContext _context;

        public MediaRevisionHistoryQueryHandler(IApplicationContext context)
        {
            _context = context;
        }

        protected override IEnumerable<MediaRevisionHistoryDto> Handle(MediaRevisionHistoryQuery request)
        {
            var mediaChanges = _context.MediaChanges
                .Where(mc => mc.MediaInstanceId == request.MediaInstanceId)
                .Select(mc => new MediaRevisionHistoryDto(mc.UserId, mc.Type, mc.Timestamp));

            return mediaChanges;
        }
    }
}
