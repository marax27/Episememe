using Episememe.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace Episememe.Application.Filtering.BaseFiltering
{
    public class FavoriteMediaFilter : IFilter<MediaInstance>
    {
        private readonly string? _userId;

        public FavoriteMediaFilter(string userId)
        {
            _userId = userId;
        }

        public ReadOnlyCollection<MediaInstance> Filter(ReadOnlyCollection<MediaInstance> instances)
        {
            return ConsiderFavorite(instances);
        }

        private ReadOnlyCollection<MediaInstance> ConsiderFavorite(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (!string.IsNullOrEmpty(_userId))
            {
                filteredMedia = mediaInstances.Where(mi =>
                        mi.FavoriteMedia.Select(fm => fm.UserId).Contains(_userId)
                        )
                    .ToList()
                    .AsReadOnly();
            }

            return filteredMedia;
        }
    }
}
