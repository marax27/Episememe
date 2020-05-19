using System.Collections.ObjectModel;

namespace Episememe.Application.Filtering
{
    public class FilterChain<T> : IFilter<T>
    {
        private readonly IFilter<T>[] _filters;

        public FilterChain(params IFilter<T>[] filters)
        {
            _filters = filters;
        }

        public ReadOnlyCollection<T> Filter(ReadOnlyCollection<T> instances)
        {
            var result = instances;

            foreach (var filter in _filters)
                result = filter.Filter(result);

            return result;
        }
    }
}
