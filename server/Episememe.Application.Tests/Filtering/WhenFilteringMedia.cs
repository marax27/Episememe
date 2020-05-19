using Episememe.Application.Features.SearchMedia;
using Episememe.Application.Filtering.BaseFiltering;
using Episememe.Application.Tests.Helpers.Contexts.Filtering;
using Episememe.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Episememe.Application.Tests.Filtering
{
    public class WhenFilteringMedia
    {
        [Fact]
        public void GivenNullFilterArguments_AllMediaAreReturned()
        {
            var mediaInstances = new TagInclusionExclusionTestsDbSet().Instances;

            var filteredMedia = GetFilteredMedia(new SearchMediaData(), mediaInstances);

            filteredMedia.Should().HaveCount(mediaInstances.Count());
        }

        [Fact]
        public void GivenExistingTag_ConnectedMediaAreReturned()
        {
            string[] includedTags = { "usa" };
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags,
            };
            var mediaInstances = new TagInclusionExclusionTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().ContainSingle(mi => mi.Id == "1");
        }

        [Fact]
        public void GivenNonexistentTag_NoMediaIsReturned()
        {
            string[] includedTags = {"politics"};
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags,
            };
            var mediaInstances = new TagInclusionExclusionTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenTagsToExclude_MediaWithoutExcludedTagsAreReturned()
        {
            string[] excludedTags = { "usa", "sport" };
            var searchMedia = new SearchMediaData()
            {
                ExcludedTags = excludedTags
            };
            var mediaInstances = new TagInclusionExclusionTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().ContainSingle(mi => mi.Id == "3");
        }

        [Fact]
        public void GivenBothIncludedAndExcludedTags_CorrectMediaAreReturned()
        {
            string[] includedTags = { "university" };
            string[] excludedTags = { "usa", "sport" };
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags,
                ExcludedTags = excludedTags
            };
            var mediaInstances = new TagInclusionExclusionTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().ContainSingle(mi => mi.Id == "3");
        }

        [Fact]
        public void GivenIncludedAndExcludedTagsBelongingToTheSameMedia_NoMediaAreReturned()
        {
            string[] includedTags = { "germany" };
            string[] excludedTags = { "usa", "sport" };
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags,
                ExcludedTags = excludedTags
            };
            var mediaInstances = new TagInclusionExclusionTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenTimeRange_MediaCreatedInTimeRangeAreReturned()
        {
            var timeRangeStart = new DateTime(2007, 6, 1, 0, 0, 0);
            var timeRangeEnd = new DateTime(2008, 9, 1, 0, 0, 0);
            var searchMedia = new SearchMediaData()
            {
                TimeRangeStart = timeRangeStart,
                TimeRangeEnd = timeRangeEnd
            };
            var mediaInstances = new TimeRangeTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().HaveCount(2);
            filteredMedia.Should().Contain(mi => mi.Id == "2");
            filteredMedia.Should().Contain(mi => mi.Id == "3");
        }

        [Fact]
        public void GivenTimeRangeStart_MediaCreatedAfterAreReturned()
        {
            var timeRangeStart = new DateTime(2008, 3, 1, 0, 0, 0);
            var searchMedia = new SearchMediaData()
            {
                TimeRangeStart = timeRangeStart
            };
            var mediaInstances = new TimeRangeTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().HaveCount(2);
            filteredMedia.Should().Contain(mi => mi.Id == "1");
            filteredMedia.Should().Contain(mi => mi.Id == "3");
        }

        [Fact]
        public void GivenTimeRangeEnd_MediaCreatedBeforeAreReturned()
        {
            var timeRangeEnd = new DateTime(2008, 9, 1, 0, 0, 0);
            var searchMedia = new SearchMediaData()
            {
                TimeRangeEnd = timeRangeEnd
            };

            var mediaInstances = new TimeRangeTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().HaveCount(2);
            filteredMedia.Should().Contain(mi => mi.Id == "2");
            filteredMedia.Should().Contain(mi => mi.Id == "3");
        }

        [Fact]
        public void GivenUserId_PublicMediaAndUsersPrivateMediaAreReturned()
        {
            var userId = "user1";
            var searchMedia = new SearchMediaData()
            {
                UserId = userId
            };
            var mediaInstances= new PrivateInstancesTestsDbSet().Instances;
            var filteredMedia = GetFilteredMedia(searchMedia, mediaInstances);

            filteredMedia.Should().HaveCount(3);
            filteredMedia.Should().Contain(mi => mi.Id == "1");
            filteredMedia.Should().Contain(mi => mi.Id == "2");
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

            filteredMedia.Should().ContainSingle(mi => mi.Id == "5");
        }


        private ISet<MediaInstance> GetFilteredMedia(SearchMediaData searchMedia, DbSet<MediaInstance> mediaInstances)
        {
            var filteredMedia = new MediaFilter(searchMedia).Filter(mediaInstances);
            return filteredMedia.ToHashSet();
        }
    }
}
