using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class TagInstanceDto
    {
        public string Name { get; }
        public string? Description { get; }
        public IEnumerable<string> Children { get; }
        public IEnumerable<string> Parents { get; }

        public TagInstanceDto(string name, string? description,
            IEnumerable<string> children, IEnumerable<string> parents)
        {
            Name = name;
            Description = description;
            Children = children;
            Parents = parents;
        }
    }
}
