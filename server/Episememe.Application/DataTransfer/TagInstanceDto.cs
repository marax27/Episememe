using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class TagInstanceDto
    {
        public string Tag { get; }
        public string Description { get; }
        
        public TagInstanceDto(string tag, string description)
        {
            Tag = tag;
            Description = description;
    
        }
    }
}
