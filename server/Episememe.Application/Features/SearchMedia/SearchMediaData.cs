﻿using System;
using System.Collections.Generic;

namespace Episememe.Application.Features.SearchMedia
{
    public class SearchMediaData
    {
        public IEnumerable<string>? IncludedTags { get; set; }
        public IEnumerable<string>? ExcludedTags { get; set; }
        public DateTime? TimeRangeStart { get; set; }
        public DateTime? TimeRangeEnd { get; set; }
        public string? UserId { get; set; }
        public bool FavoritesOnly { get; set; }
    }
}
