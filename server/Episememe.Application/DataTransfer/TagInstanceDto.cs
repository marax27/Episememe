using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class TagInstanceDto
    {
        public string Name { get; }
        public string Description { get; }
        
        public TagInstanceDto(string name, string description)
        {
            Name = name;
            Description = description;
    
        }
    }
}
