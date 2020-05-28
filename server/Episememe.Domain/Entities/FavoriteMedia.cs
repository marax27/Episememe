namespace Episememe.Domain.Entities
{
    public class FavoriteMedia
    {
        public string MediaInstanceId { get; set; } = null!;
        public MediaInstance MediaInstance { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}
