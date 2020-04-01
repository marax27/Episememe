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
        public string Path { get; set; } = null!;
        public int RevisionCount { get; set; }

        public ICollection<MediaTag> MediaTags { get; set; } = new HashSet<MediaTag>();
    }
}
