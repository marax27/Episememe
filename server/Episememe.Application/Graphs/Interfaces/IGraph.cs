using System.Collections.Generic;
using Episememe.Domain.Entities;

namespace Episememe.Application.Graphs.Interfaces
{
    public interface IGraph<TEntity> where TEntity : class
    {
        IVertex<TEntity> Add(Tag tag);
        IVertex<TEntity> this[string name] { get; }

        IEnumerable<TEntity> Nodes { get; }

        void SaveChanges();
        void CommitAllChanges();
    }
}