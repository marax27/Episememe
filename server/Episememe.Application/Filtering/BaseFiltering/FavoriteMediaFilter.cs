using Episememe.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace Episememe.Application.Filtering.BaseFiltering
{
    public class FavoriteMediaFilter : IFilter<MediaInstance>
    {
        private readonly string? _userId;
        private readonly bool _favoritesOnly;

        public FavoriteMediaFilter(string? userId, bool favoritesOnly)
        {
            _userId = userId;
            _favoritesOnly = favoritesOnly;
        }

        public ReadOnlyCollection<MediaInstance> Filter(ReadOnlyCollection<MediaInstance> instances)
        {
            return ConsiderFavorite(instances);
        }

        private ReadOnlyCollection<MediaInstance> ConsiderFavorite(ReadOnlyCollection<MediaInstance> mediaInstances)
        {
            var filteredMedia = mediaInstances;
            if (_userId != null && _favoritesOnly)
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
