using Episememe.Application.DataTransfer;
using Episememe.Application.Tests.Helpers.Contexts;
using Episememe.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Episememe.Application.Filtering.BaseFiltering;
using Xunit;

namespace Episememe.Application.Tests.Filtering
{
    public class WhenFilteringMedia
    {
        [Fact]
        public void GivenNullFilterArguments_AllMediaAreReturned()
        {
            var mediaInstances = new GivenThreeMediaInstancesDbSet().MediaInstances;

            var filteredMedia = GetFilteredMedia(new SearchMediaDto());

            filteredMedia.Count().Should().Be(mediaInstances.Count());
        }

        [Fact]
        public void GivenExistingTag_ConnectedMediaAreReturned()
        {
            string[] includedTags = { "usa" };
            var searchMedia = new SearchMediaDto()
            {
                IncludedTags = includedTags,
            };

            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Should().NotBeEmpty();
            filteredMedia.Count().Should().Be(1);
            filteredMedia.Single().Id.Should().Be("1");
        }

        [Fact]
        public void GivenNonexistentTag_NoMediaIsReturned()
        {
            string[] includedTags = {"politics"};
            var searchMedia = new SearchMediaDto()
            {
                IncludedTags = includedTags,
            };

            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenTagsToExclude_MediaWithoutExcludedTagsAreReturned()
        {
            string[] excludedTags = { "usa", "sport" };
            var searchMedia = new SearchMediaDto()
            {
                ExcludedTags = excludedTags
            };

            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Count().Should().Be(1);
            filteredMedia.Single().Id.Should().Be("3");
        }

        [Fact]
        public void GivenBothIncludedAndExcludedTags_CorrectMediaAreReturned()
        {
            string[] includedTags = { "university" };
            string[] excludedTags = { "usa", "sport" };
            var searchMedia = new SearchMediaDto()
            {
                IncludedTags = includedTags,
                ExcludedTags = excludedTags
            };

            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Count().Should().Be(1);
            filteredMedia.Single().Id.Should().Be("3");
        }

        [Fact]
        public void GivenIncludedAndExcludedTagsBelongingToTheSameMedia_NoMediaAreReturned()
        {
            string[] includedTags = { "germany" };
            string[] excludedTags = { "usa", "sport" };
            var searchMedia = new SearchMediaDto()
            {
                IncludedTags = includedTags,
                ExcludedTags = excludedTags
            };

            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenTimeRange_MediaCreatedInTimeRangeAreReturned()
        {
            var timeRangeStart = new DateTime(2007, 6, 1, 0, 0, 0);
            var timeRangeEnd = new DateTime(2008, 9, 1, 0, 0, 0);
            var searchMedia = new SearchMediaDto()
            {
                TimeRangeStart = timeRangeStart,
                TimeRangeEnd = timeRangeEnd
            };

            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Count().Should().Be(2);
        }

        [Fact]
        public void GivenTimeRangeStart_MediaCreatedAfterAreReturned()
        {
            var timeRangeStart = new DateTime(2008, 3, 1, 0, 0, 0);
            var searchMedia = new SearchMediaDto()
            {
                TimeRangeStart = timeRangeStart
            };

            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Count().Should().Be(2);
        }

        [Fact]
        public void GivenTimeRangeEnd_MediaCreatedBeforeAreReturned()
        {
            var timeRangeEnd = new DateTime(2008, 9, 1, 0, 0, 0);
            var searchMedia = new SearchMediaDto()
            {
                TimeRangeEnd = timeRangeEnd
            };

            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Count().Should().Be(2);
        }

        private ISet<MediaInstance> GetFilteredMedia(SearchMediaDto searchMedia)
        {
            var mediaInstances = new GivenThreeMediaInstancesDbSet().MediaInstances;
            var filteredMedia = new MediaFilter(searchMedia).Filter(mediaInstances);
            return filteredMedia.ToHashSet();
        }
    }
}
