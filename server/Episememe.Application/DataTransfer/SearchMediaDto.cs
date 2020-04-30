using System;
using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class SearchMediaDto
    {
        public IEnumerable<string>? IncludedTags { get; set; }
        public IEnumerable<string>? ExcludedTags { get; set; }
        public DateTime? TimeRangeStart { get; set; }
        public DateTime? TimeRangeEnd { get; set; }
    }
}
