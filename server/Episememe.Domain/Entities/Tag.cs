using System.Collections.Generic;
using Episememe.Domain.HelperEntities;

namespace Episememe.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<MediaTag> MediaTags { get; set; } = new HashSet<MediaTag>();
    }
}
