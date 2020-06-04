using System.Linq;
using Episememe.Application.Graphs.Interfaces;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;

namespace Episememe.Application.Graphs.Tags
{
    public class TagGraph : IGraph<Tag>
    {
        private readonly IWritableApplicationContext _context;

        public TagGraph(IWritableApplicationContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IVertex<Tag> Add(Tag tag)
        {
            _context.Tags.Add(tag);
            return new TagVertex(tag, _context);
        }

        public IVertex<Tag> this[string name]
        {
            get
            {
                var tagEntity = _context.Tags.Single(tag => tag.Name == name);
                return new TagVertex(tagEntity, _context);
            }
        }
    }
}
