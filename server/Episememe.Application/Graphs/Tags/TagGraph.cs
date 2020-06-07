using System.Collections.Generic;
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

        public IEnumerable<IVertex<Tag>> Vertices
            => _context.Tags.Select(tag => new TagVertex(tag, _context));

        public void SaveChanges()
            => _context.SaveChanges();
        
        public void CommitAllChanges()
        {
            SaveChanges();
            if (_context.Database.CurrentTransaction != null)
                _context.Database.CommitTransaction();
        }

        public IVertex<Tag> Add(Tag tag)
        {
            EnsureTransactionIsOpen();
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

        private void EnsureTransactionIsOpen()
        {
            if (_context.Database.CurrentTransaction == null)
                _context.Database.BeginTransaction();
        }
    }
}
