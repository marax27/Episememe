using System.Linq;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Episememe.Application.TagGraph
{
    public interface ITagGraphService
    {
        TagVertex this[string name] { get; }

        TagVertex Create(Tag tag);

        void SaveChanges();
    }

    public class TagGraphService : ITagGraphService
    {
        private readonly IWritableApplicationContext _context;

        public TagGraphService(IWritableApplicationContext context)
            => _context = context;

        public TagVertex this[string name]
            => new TagVertex(LoadTagByName(name));

        public TagVertex Create(Tag tag)
        {
            _context.Tags.Add(tag);
            return new TagVertex(tag);
        }

        public void SaveChanges()
            => _context.SaveChanges();

        private Tag LoadTagByName(string name)
        {
            return _context.Tags
                .Where(tag => tag.Name == name)
                .Include(tag => tag.Successors)
                    .ThenInclude(c => c.Successor)
                .Include(tag => tag.Ancestors)
                    .ThenInclude(c => c.Ancestor)
                .Single();
        }
    }
}
