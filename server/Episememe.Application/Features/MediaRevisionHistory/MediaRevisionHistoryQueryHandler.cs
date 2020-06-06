using Episememe.Application.DataTransfer;
using Episememe.Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using Episememe.Application.Exceptions;

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
            var mediaInstance = _context.MediaInstances.Find(request.MediaInstanceId);

            if (mediaInstance == null)
                return new List<MediaRevisionHistoryDto>();

            if (mediaInstance.IsPrivate && mediaInstance.AuthorId != request.UserId)
                throw new MediaDoesNotBelongToUserException(request.UserId ?? string.Empty);

            return _context.MediaChanges
                .Where(mc => mc.MediaInstanceId == request.MediaInstanceId)
                .Select(mc => new MediaRevisionHistoryDto(mc.UserId, mc.Type, mc.Timestamp));
        }
    }
}
