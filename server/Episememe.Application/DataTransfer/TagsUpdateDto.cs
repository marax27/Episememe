using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class TagsUpdateDto
    {
        public IEnumerable<string>? Tags { get; set; }
    }
}