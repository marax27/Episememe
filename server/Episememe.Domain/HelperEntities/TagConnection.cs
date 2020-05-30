using Episememe.Domain.Entities;

namespace Episememe.Domain.HelperEntities
{
    public class TagConnection
    {
        public Tag Ancestor { get; set; } = null!;
        public int AncestorId { get; set; }
        public Tag Successor { get; set; } = null!;
        public int SuccessorId { get; set; }
        public int Depth { get; set; }
    }
}
