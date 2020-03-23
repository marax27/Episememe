using Episememe.Domain.Entities;

namespace Episememe.Domain.HelperEntities
{
    public class MediaTag
    {
        public string MediaInstanceId { get; set; }
        public MediaInstance MediaInstance { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
