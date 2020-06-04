using System.Linq;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;

namespace Episememe.Application.TagGraph
{
    public interface ITagGraphService
    {
        TagVertex Add(Tag tag);
        TagVertex this[string name] { get; }

        void SaveChanges();
    }

    public class TagGraphService : ITagGraphService
    {
        private readonly IWritableApplicationContext _context;

        public TagGraphService(IWritableApplicationContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public TagVertex Add(Tag tag)
        {
            _context.Tags.Add(tag);
            return new TagVertex(tag, _context);
        }

        public TagVertex this[string name]
        {
            get
            {
                var tagEntity = _context.Tags.Single(tag => tag.Name == name);
                return new TagVertex(tagEntity, _context);
            }
        }
    }
}
