using Episememe.Application.DataTransfer;
using MediatR;
using System;
using System.Collections.Generic;

namespace Episememe.Application.Features.MediaRevisionHistory
{
    public class MediaRevisionHistoryQuery : IRequest<IEnumerable<MediaRevisionHistoryDto>>
    {
        public string MediaInstanceId { get; }
        public string UserId { get; }

        private MediaRevisionHistoryQuery(string mediaInstanceId, string userId)
        {
            MediaInstanceId = mediaInstanceId;
            UserId = userId;
        }

        public static MediaRevisionHistoryQuery Create(string mediaInstanceId, string userId)
        {
            if (mediaInstanceId == null)
                throw new ArgumentNullException(nameof(mediaInstanceId));
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            return new MediaRevisionHistoryQuery(mediaInstanceId, userId);
        }
    }
}
