using System;
using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class SearchMediaDto
    {
        public IEnumerable<string> IncludedTags { get; set; } = null!;
        public IEnumerable<string> ExcludedTags { get; set; } = null!;
        public DateTime? TimeRangeStart { get; set; }
        public DateTime? TimeRangeEnd { get; set; }
    }
}
