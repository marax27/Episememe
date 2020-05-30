using System.Collections.Generic;

namespace Episememe.Application.Common
{
    public static class ICollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                destination.Add(item);
            }
        }
    }
}
