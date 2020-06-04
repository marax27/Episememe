using Episememe.Domain.Entities;

namespace Episememe.Application.Graphs.Interfaces
{
    public interface IGraph<TEntity> where TEntity : class
    {
        IVertex<TEntity> Add(Tag tag);
        IVertex<TEntity> this[string name] { get; }

        void SaveChanges();
    }
}