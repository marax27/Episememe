using System;
using System.Collections.Generic;
using Episememe.Domain.HelperEntities;

namespace Episememe.Domain.Entities
{
    public class MediaInstance
    {
        public string Id { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string DataType { get; set; } = null!;
        public string? AuthorId { get; set; }
        public bool IsPrivate { get; set; }

        public ICollection<MediaTag> MediaTags { get; set; } = new HashSet<MediaTag>();
        public ICollection<FavoriteMedia> FavoriteMedia { get; set; } = new HashSet<FavoriteMedia>();
        public ICollection<MediaChange> MediaChanges { get; set; } = new HashSet<MediaChange>();
    }
}
