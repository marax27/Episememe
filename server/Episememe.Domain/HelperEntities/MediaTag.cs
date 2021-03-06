﻿using Episememe.Domain.Entities;

namespace Episememe.Domain.HelperEntities
{
    public class MediaTag
    {
        public string MediaInstanceId { get; set; } = null!;
        public MediaInstance MediaInstance { get; set; } = null!;
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
