using System.Collections.Generic;

namespace Episememe.Application.Graphs.Interfaces
{
    public interface IGraph<TEntity> where TEntity : class
    {
        IVertex<TEntity> Add(TEntity entity);
        IVertex<TEntity> this[string name] { get; }

        IEnumerable<IVertex<TEntity>> Vertices { get; }

        void SaveChanges();
        void CommitAllChanges();
    }
}