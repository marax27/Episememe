using Episememe.Application.Features.MediaFiltering;
using Episememe.Application.Tests.Helpers;
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
            var mediaInstances = DbSetMockFactory.SampleMediaInstancesDbSet();

            var filteredMedia = new MediaFilter(mediaInstances)
                .IncludeTags(null)
                .ExcludeTags(null)
                .InTimeRange(null, null)
                .Result()
                .AsEnumerable();

            filteredMedia.Count().Should().Be(mediaInstances.Count());
        }

        [Fact]
        public void GivenExistingTag_ConnectedMediaAreReturned()
        {
            var mediaInstances = DbSetMockFactory.SampleMediaInstancesDbSet();
            string[] includedTags = { "usa" };

            var filteredMedia = new MediaFilter(mediaInstances)
                .IncludeTags(includedTags)
                .Result()
                .AsEnumerable();

            filteredMedia.Should().NotBeEmpty();
            filteredMedia.Count().Should().Be(1);
            filteredMedia.Single().Id.Should().Be("1");
        }

        [Fact]
        public void GivenNonexistentTag_NoMediaIsReturned()
        {
            var mediaInstances = DbSetMockFactory.SampleMediaInstancesDbSet();
            string[] includedTags = {"politics"};

            var filteredMedia = new MediaFilter(mediaInstances)
                .IncludeTags(includedTags)
                .Result()
                .AsEnumerable();

            filteredMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenTagsToExclude_MediaWithoutExcludedTagsAreReturned()
        {
            var mediaInstances = DbSetMockFactory.SampleMediaInstancesDbSet();
            string[] excludedTags = { "usa", "sport" };

            var filteredMedia = new MediaFilter(mediaInstances)
                .ExcludeTags(excludedTags)
                .Result()
                .AsEnumerable();

            filteredMedia.Count().Should().Be(1);
            filteredMedia.Single().Id.Should().Be("3");
        }

        [Fact]
        public void GivenBothIncludedAndExcludedTags_CorrectMediaAreReturned()
        {
            var mediaInstances = DbSetMockFactory.SampleMediaInstancesDbSet();
            string[] includedTags = { "university" };
            string[] excludedTags = { "usa", "sport" };

            var filteredMedia = new MediaFilter(mediaInstances)
                .IncludeTags(includedTags)
                .ExcludeTags(excludedTags)
                .Result()
                .AsEnumerable();

            filteredMedia.Count().Should().Be(1);
            filteredMedia.Single().Id.Should().Be("3");
        }

        [Fact]
        public void GivenIncludedAndExcludedTagsBelongingToTheSameMedia_NoMediaAreReturned()
        {
            var mediaInstances = DbSetMockFactory.SampleMediaInstancesDbSet();
            string[] includedTags = { "germany" };
            string[] excludedTags = { "usa", "sport" };

            var filteredMedia = new MediaFilter(mediaInstances)
                .IncludeTags(includedTags)
                .ExcludeTags(excludedTags)
                .Result()
                .AsEnumerable();

            filteredMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenTimeRange_MediaCreatedInTimeRangeAreReturned()
        {
            var mediaInstances = DbSetMockFactory.SampleMediaInstancesDbSet();
            var timeRangeStart = DateTime.Today.AddYears(-2).AddMonths(-6);
            var timeRangeEnd = DateTime.Today.AddYears(-1).AddMonths(-3);

            var filteredMedia = new MediaFilter(mediaInstances)
                .InTimeRange(timeRangeStart, timeRangeEnd)
                .Result()
                .AsEnumerable();

            filteredMedia.Count().Should().Be(2);
        }

        [Fact]
        public void GivenTimeRangeStart_MediaCreatedAfterAreReturned()
        {
            var mediaInstances = DbSetMockFactory.SampleMediaInstancesDbSet();
            var timeRangeStart = DateTime.Today.AddYears(-1).AddMonths(-9);

            var filteredMedia = new MediaFilter(mediaInstances)
                .InTimeRange(timeRangeStart, null)
                .Result()
                .AsEnumerable();

            filteredMedia.Count().Should().Be(2);
        }

        [Fact]
        public void GivenTimeRangeEnd_MediaCreatedBeforeAreReturned()
        {
            var mediaInstances = DbSetMockFactory.SampleMediaInstancesDbSet();
            var timeRangeEnd = DateTime.Today.AddYears(-1).AddMonths(-3);

            var filteredMedia = new MediaFilter(mediaInstances)
                .InTimeRange(null, timeRangeEnd)
                .Result()
                .AsEnumerable();

            filteredMedia.Count().Should().Be(2);
        }
    }
}
