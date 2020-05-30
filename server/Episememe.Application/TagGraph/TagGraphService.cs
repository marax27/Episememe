using System.Linq;
using Episememe.Application.Interfaces;

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
            => new TagVertex(_context.Tags.Single(tag => tag.Name == name));
    }
}
