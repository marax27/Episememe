namespace Episememe.Domain.HelperEntities
{
    public class TagConnection
    {
        public int Id { get; set; }
        public int EntryEdgeId { get; set; }
        public int DirectEdgeId { get; set; }
        public int ExitEdgeId { get; set; }
        public int Successor { get; set; }
        public int Ancestor { get; set; }
        public int Hops { get; set; }
    }
}
