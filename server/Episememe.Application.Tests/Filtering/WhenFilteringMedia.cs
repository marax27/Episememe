using Episememe.Application.DataTransfer;
using Episememe.Application.Features.MediaFiltering;
using Episememe.Application.Tests.Helpers;
using Episememe.Application.Tests.Helpers.Contexts;
using Episememe.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Episememe.Application.Tests.Filtering
{
    public class WhenFilteringMedia
    {
        [Fact]
        public void GivenNullFilterArguments_AllMediaAreReturned()
        {
            var mediaInstances = new GivenThreeMediaInstancesDbSet().MediaInstances;

            var filteredMedia = GetFilteredMedia(null, null, null, null);

            filteredMedia.Count().Should().Be(mediaInstances.Count());
        }

        [Fact]
        public void GivenExistingTag_ConnectedMediaAreReturned()
        {
            string[] includedTags = { "usa" };

            var filteredMedia = GetFilteredMedia(includedTags, null, null, null);

            filteredMedia.Should().NotBeEmpty();
            filteredMedia.Count().Should().Be(1);
            filteredMedia.Single().Id.Should().Be("1");
        }

        [Fact]
        public void GivenNonexistentTag_NoMediaIsReturned()
        {
            string[] includedTags = {"politics"};

            var filteredMedia = GetFilteredMedia(includedTags, null, null, null);

            filteredMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenTagsToExclude_MediaWithoutExcludedTagsAreReturned()
        {
            string[] excludedTags = { "usa", "sport" };

            var filteredMedia = GetFilteredMedia(null, excludedTags, null, null);

            filteredMedia.Count().Should().Be(1);
            filteredMedia.Single().Id.Should().Be("3");
        }

        [Fact]
        public void GivenBothIncludedAndExcludedTags_CorrectMediaAreReturned()
        {
            string[] includedTags = { "university" };
            string[] excludedTags = { "usa", "sport" };

            var filteredMedia = GetFilteredMedia(includedTags, excludedTags, null, null);

            filteredMedia.Count().Should().Be(1);
            filteredMedia.Single().Id.Should().Be("3");
        }

        [Fact]
        public void GivenIncludedAndExcludedTagsBelongingToTheSameMedia_NoMediaAreReturned()
        {
            string[] includedTags = { "germany" };
            string[] excludedTags = { "usa", "sport" };

            var filteredMedia = GetFilteredMedia(includedTags, excludedTags, null, null);

            filteredMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenTimeRange_MediaCreatedInTimeRangeAreReturned()
        {
            var timeRangeStart = DateTime.Today.AddYears(-2).AddMonths(-6);
            var timeRangeEnd = DateTime.Today.AddYears(-1).AddMonths(-3);

            var filteredMedia = GetFilteredMedia(null, null, timeRangeStart, timeRangeEnd);

            filteredMedia.Count().Should().Be(2);
        }

        [Fact]
        public void GivenTimeRangeStart_MediaCreatedAfterAreReturned()
        {
            var timeRangeStart = DateTime.Today.AddYears(-1).AddMonths(-9);

            var filteredMedia = GetFilteredMedia(null, null, timeRangeStart, null);

            filteredMedia.Count().Should().Be(2);
        }

        [Fact]
        public void GivenTimeRangeEnd_MediaCreatedBeforeAreReturned()
        {
            var timeRangeEnd = DateTime.Today.AddYears(-1).AddMonths(-3);

            var filteredMedia = GetFilteredMedia(null, null, null, timeRangeEnd);

            filteredMedia.Count().Should().Be(2);
        }

        private IEnumerable<MediaInstance> GetFilteredMedia(IEnumerable<string> includedTags, IEnumerable<string> excludedTags,
            DateTime? timeRangeStart, DateTime? timeRangeEnd)
        {
            var mediaInstances = new GivenThreeMediaInstancesDbSet().MediaInstances;
            var searchMedia = new SearchMediaDto(includedTags, excludedTags, timeRangeStart, timeRangeEnd);
            return new MediaFilter(searchMedia).Filter(mediaInstances);
        }
    }
}
