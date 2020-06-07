using Episememe.Domain.HelperEntities;
using System;

namespace Episememe.Application.DataTransfer
{
    public class MediaRevisionHistoryDto
    {
        public string UserId { get; }
        public string MediaChangeType { get; }
        public DateTime TimeStamp { get; }

        public MediaRevisionHistoryDto(string userId, MediaChangeType mediaChangeType, DateTime timeStamp)
        {
            UserId = userId;
            MediaChangeType = mediaChangeType.ToString();
            TimeStamp = timeStamp;
        }
    }
}
