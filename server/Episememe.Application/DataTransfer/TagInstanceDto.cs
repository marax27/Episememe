using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class TagInstanceDto
    {
        public string Name { get; }
        public string? Description { get; }
        public IEnumerable<string> Successors { get; }
        public IEnumerable<string> Ancestors { get; }

        public TagInstanceDto(string name, string? description,
            IEnumerable<string> successors, IEnumerable<string> ancestors)
        {
            Name = name;
            Description = description;
            Successors = successors;
            Ancestors = ancestors;
        }
    }
}
