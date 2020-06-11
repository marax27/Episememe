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
    public class WhenFilteringMediaByTimeRange
    {
        [Fact]
        public void GivenNullFilterArguments_AllMediaAreReturned()
        {
            var mediaInstances = new TimeRangeTestsDbSet().Instances;

            var filteredMedia = GetFilteredMedia(new SearchMediaData(), mediaInstances);

            filteredMedia.Should().HaveCount(mediaInstances.Count());
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

        private ISet<MediaInstance> GetFilteredMedia(SearchMediaData searchMedia, DbSet<MediaInstance> mediaInstances)
        {
            var timeRangeFilter = new TimeRangeFilter(searchMedia.TimeRangeStart, searchMedia.TimeRangeEnd);
            var filteredMedia = timeRangeFilter.Filter(mediaInstances.ToList().AsReadOnly())
                .ToHashSet();

            return filteredMedia;
        }
    }
}
