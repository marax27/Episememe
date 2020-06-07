using System;
using Episememe.Domain.Entities;

namespace Episememe.Domain.HelperEntities
{
    public class MediaChange
    {
        public int Id { get; set; }
        public string MediaInstanceId { get; set; } = null!;
        public MediaInstance MediaInstance { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public MediaChangeType Type { get; set; }
    }

    public enum MediaChangeType
    {
        Create,
        Update,
        Remove
    }
}
