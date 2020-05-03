using System;

namespace Episememe.Domain.Entities
{
    public class BrowseToken
    {
        public string Id { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
