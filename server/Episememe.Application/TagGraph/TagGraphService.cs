using System.Linq;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Episememe.Application.TagGraph
{
    public interface ITagGraphService
    {
        TagVertex this[string name] { get; }
    }

    public class TagGraphService : ITagGraphService
    {
        private readonly IWritableApplicationContext _context;

        public TagGraphService(IWritableApplicationContext context)
            => _context = context;

        public void SaveChanges()
            => _context.SaveChanges();

        public TagVertex this[string name]
            => new TagVertex(LoadTagByName(name));

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
