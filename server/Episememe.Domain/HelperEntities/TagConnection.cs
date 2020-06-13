using Episememe.Domain.Entities;

namespace Episememe.Domain.HelperEntities
{
    public class TagConnection
    {
        public int Id { get; set; }
        public int EntryEdgeId { get; set; }
        public int DirectEdgeId { get; set; }
        public int ExitEdgeId { get; set; }
        public int SuccessorId { get; set; }
        public int AncestorId { get; set; }
        public int Hops { get; set; }

        public Tag Successor { get; set; } = null!;
        public Tag Ancestor { get; set; } = null!;
    }
}
