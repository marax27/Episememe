using Episememe.Application.Features.SearchMedia;
using Episememe.Application.Filtering.BaseFiltering;
using Episememe.Application.Tests.Helpers.Contexts.Filtering;
using Episememe.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Episememe.Application.Tests.Filtering
{
    public class WhenFilteringMediaByFavorite
    {
        [Fact]
        public void GivenNullFilterArguments_AllMediaAreReturned()
        {
            var givenMediaInstances = new FavoriteMediaTestsDbSet().Instances;

            var filteredMedia = GetFilteredMedia(new SearchMediaData(), givenMediaInstances);

            filteredMedia.Should().HaveCount(givenMediaInstances.Count());
        }

        [Fact]
        public void GivenUserAndMediaDataSet_UsersFavoriteMediaAreReturned()
        {
            var givenUserId = "user1";
            var givenMediaInstances = new FavoriteMediaTestsDbSet().Instances;
            var givenSearchMediaData = new SearchMediaData()
            {
                UserId = givenUserId
            };

            var filteredMedia = GetFilteredMedia(givenSearchMediaData, givenMediaInstances);

            filteredMedia.Should().HaveCount(2);
            filteredMedia.Should().Contain(mi => mi.Id == "1");
            filteredMedia.Should().Contain(mi => mi.Id == "2");
        }

        private ISet<MediaInstance> GetFilteredMedia(SearchMediaData searchMedia, DbSet<MediaInstance> mediaInstances)
        {
            var favoriteMediaFilter = new FavoriteMediaFilter(searchMedia.UserId);
            var filteredMedia = favoriteMediaFilter.Filter(mediaInstances.ToList().AsReadOnly())
                .ToHashSet();

            return filteredMedia;
        }
    }
}
