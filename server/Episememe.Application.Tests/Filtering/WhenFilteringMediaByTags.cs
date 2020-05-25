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
    public class WhenFilteringMediaByTags
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

        private ISet<MediaInstance> GetFilteredMedia(SearchMediaData searchMedia, DbSet<MediaInstance> mediaInstances)
        {
            var mediaFilter = new MediaFilter(searchMedia.IncludedTags, searchMedia.ExcludedTags, 
                searchMedia.TimeRangeStart, searchMedia.TimeRangeEnd);
            var filteredMedia = mediaFilter.Filter(mediaInstances.ToList().AsReadOnly())
                .ToHashSet();

            return filteredMedia;
        }
    }
}
