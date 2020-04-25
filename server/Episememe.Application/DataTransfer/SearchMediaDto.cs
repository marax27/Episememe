using System;
using System.Collections.Generic;
using System.Text;

namespace Episememe.Application.DataTransfer
{
    public class SearchMediaDto
    {
        public IEnumerable<string> IncludedTags { get; } = null!;
        public IEnumerable<string> ExcludedTags { get; } = null!;
        public DateTime? TimeRangeStart { get; } = null;
        public DateTime? TimeRangeEnd { get; } = null;

        public SearchMediaDto(IEnumerable<string> includedTags, IEnumerable<string> excludedTags,
            DateTime? timeRangeStart, DateTime? timeRangeEnd)
        {
            IncludedTags = includedTags;
            ExcludedTags = excludedTags;
            TimeRangeStart = timeRangeStart;
            TimeRangeEnd = timeRangeEnd;
        }
    }
}
