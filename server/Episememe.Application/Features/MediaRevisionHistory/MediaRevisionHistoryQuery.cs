using Episememe.Application.DataTransfer;
using MediatR;
using System;
using System.Collections.Generic;

namespace Episememe.Application.Features.MediaRevisionHistory
{
    public class MediaRevisionHistoryQuery : IRequest<IEnumerable<MediaRevisionHistoryDto>>
    {
        public string MediaInstanceId { get; }

        private MediaRevisionHistoryQuery(string mediaInstanceId)
        {
            MediaInstanceId = mediaInstanceId;
        }

        public static MediaRevisionHistoryQuery Create(string mediaInstanceId)
        {
            if (mediaInstanceId == null)
                throw new ArgumentNullException(nameof(mediaInstanceId));

            return new MediaRevisionHistoryQuery(mediaInstanceId);
        }
    }
}
