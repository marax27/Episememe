using System.Collections.Generic;

namespace Episememe.Application.Graphs.Interfaces
{
    public interface IVertex<TEntity> where TEntity : class
    {
        TEntity Entity { get; }
        IEnumerable<TEntity> Successors { get; }
        IEnumerable<TEntity> Ancestors { get; }

        void AddParent(IVertex<TEntity> newParent);
        void AddChild(IVertex<TEntity> newChild)
            => newChild.AddParent(this);

        void DeleteParent(IVertex<TEntity> parent);
        void DeleteChild(IVertex<TEntity> child)
            => child.DeleteParent(this);
    }
}