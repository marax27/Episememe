using Episememe.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace Episememe.Application.Filtering.BaseFiltering
{
    public class PrivateMediaFilter : IFilter<MediaInstance>
    {
        private readonly string? _userId;

        public PrivateMediaFilter(string? userId)
        {
            _userId = userId;
        }

        public ReadOnlyCollection<MediaInstance> Filter(ReadOnlyCollection<MediaInstance> instances)
        {
            return ConsiderPrivate(instances);
        }

        private ReadOnlyCollection<MediaInstance> ConsiderPrivate(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (!string.IsNullOrEmpty(_userId))
            {
                filteredMedia = mediaInstances.Where(mi => !mi.IsPrivate || mi.AuthorId == _userId)
                    .ToList()
                    .AsReadOnly();
            }

            return filteredMedia;
        }
    }
}
