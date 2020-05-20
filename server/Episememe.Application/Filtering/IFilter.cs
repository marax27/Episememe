using System.Collections.ObjectModel;

namespace Episememe.Application.Filtering
{
    public interface IFilter<T>
    {
        ReadOnlyCollection<T> Filter(ReadOnlyCollection<T> instances);
    }
}
