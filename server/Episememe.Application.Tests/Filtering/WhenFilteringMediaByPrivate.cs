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
    public class WhenFilteringMediaByPrivate
    {
        [Fact]
        public void GivenNullFilterArguments_AllMediaAreReturned()
        {
            var mediaInstances = new PrivateInstancesTestsDbSet().Instances;

            var filteredMedia = GetFilteredMedia(new SearchMediaData(), mediaInstances);

            filteredMedia.Should().HaveCount(mediaInstances.Count());
        }

        [Fact]
        public void GivenUserId_PublicMediaAndUsersPrivateMediaAreReturned()
        {
            var userId = "user1";
            var searchMedia = new SearchMediaData()
            {
                UserId = userId
            };
            var mediaInstances = new PrivateInstancesTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().HaveCount(4);
            filteredMedia.Should().Contain(mi => mi.Id == "1");
            filteredMedia.Should().Contain(mi => mi.Id == "2");
            filteredMedia.Should().Contain(mi => mi.Id == "4");
            filteredMedia.Should().Contain(mi => mi.Id == "5");
        }

        [Fact]
        public void GivenUserIdNotConnectedToAnyMedia_PublicMediaAreReturned()
        {
            var userId = "randomUser";
            var searchMedia = new SearchMediaData()
            {
                UserId = userId
            };
            var mediaInstances = new PrivateInstancesTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().HaveCount(3);
            filteredMedia.Should().Contain(mi => mi.Id == "2");
            filteredMedia.Should().Contain(mi => mi.Id == "4");
            filteredMedia.Should().Contain(mi => mi.Id == "5");
        }

        private ISet<MediaInstance> GetFilteredMedia(SearchMediaData searchMedia, DbSet<MediaInstance> mediaInstances)
        {
            var privateMediaFilter = new PrivateMediaFilter(searchMedia.UserId);
            var filteredMedia = privateMediaFilter.Filter(mediaInstances.ToList().AsReadOnly())
                .ToHashSet();

            return filteredMedia;
        }
    }
}
