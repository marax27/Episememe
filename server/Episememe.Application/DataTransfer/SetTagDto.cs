using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class SetTagDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<string>? Children { get; set; }
        public IEnumerable<string>? Parents { get; set; }
    }
}
