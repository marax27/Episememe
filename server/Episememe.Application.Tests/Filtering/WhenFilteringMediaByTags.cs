using Episememe.Application.Features.SearchMedia;
using Episememe.Application.Filtering.BaseFiltering;
using Episememe.Application.Graphs.Interfaces;
using Episememe.Application.Graphs.Tags;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using FluentAssertions;
using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using Xunit;

namespace Episememe.Application.Tests.Filtering
{
    public class WhenFilteringMediaByTags : IDisposable
    {
        private readonly IWritableApplicationContext _context;
        private readonly DbConnection _connection;
        private readonly IGraph<Tag> _tagGraph;

        public WhenFilteringMediaByTags()
        {
            (_context, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _tagGraph = new TagGraph(_context);
            FillDatabase();
        }

        [Fact]
        public void GivenNullFilterArguments_AllMediaAreReturned()
        {
            var filteredMedia = GetFilteredMedia(new SearchMediaData());

            filteredMedia.Should().BeEquivalentTo(_context.MediaInstances);
        }

        [Fact]
        public void GivenExistingTag_ConnectedMediaAreReturned()
        {
            string[] includedTags = { "books" };
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags,
            };
            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Should().ContainSingle(mi => mi.Id == "1");
        }

        [Fact]
        public void GivenNonexistentIncludedTag_NoMediaIsReturned()
        {
            string[] includedTags = { "football" };
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags
            };
            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenNonexistentExcludedTag_AllMediaAreReturned()
        {
            string[] excludedTags = { "football" };
            var searchMedia = new SearchMediaData()
            {
                ExcludedTags = excludedTags
            };
            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Should().BeEquivalentTo(_context.MediaInstances);
        }

        [Fact]
        public void GivenTagsToExclude_MediaWithoutExcludedTagsAreReturned()
        {
            string[] excludedTags = { "books", "sport" };
            var searchMedia = new SearchMediaData()
            {
                ExcludedTags = excludedTags
            };
            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Should().ContainSingle(mi => mi.Id == "3");
        }

        [Fact]
        public void GivenBothIncludedAndExcludedTags_CorrectMediaAreReturned()
        {
            string[] includedTags = { "university" };
            string[] excludedTags = { "books", "sport" };
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags,
                ExcludedTags = excludedTags
            };
            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Should().ContainSingle(mi => mi.Id == "3");
        }

        [Fact]
        public void GivenIncludedAndExcludedTagsBelongingToTheSameMedia_NoMediaAreReturned()
        {
            string[] includedTags = { "islands" };
            string[] excludedTags = { "books", "sport" };
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags,
                ExcludedTags = excludedTags
            };
            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenIncludedTags_MediaWithTagOrSuccessorTagsAreReturned()
        {
            string[] includedTags = { "politics" };
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags
            };
            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Select(mi => mi.Id).Should().HaveCount(5)
                .And.Contain(new[] { "11", "12", "13", "14", "16" });
        }

        [Fact]
        public void GivenExcludedTags_MediaWithoutTagOrSuccessorTagsAreReturned()
        {
            string[] excludedTags = { "famous building" };
            var searchMedia = new SearchMediaData()
            {
                ExcludedTags = excludedTags
            };
            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Select(mi => mi.Id).Should().HaveCount(7)
                .And.Contain(new[] { "1", "2", "3", "11", "12", "13", "14" });
        }

        [Fact]
        public void GivenIncludedParentTagAndExcludedChildTag_MediaWithTagOrNonExcludedSuccessorsAreReturned()
        {
            string[] includedTags = { "politics" };
            string[] excludedTags = { "usa" };
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags,
                ExcludedTags = excludedTags
            };
            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Select(mi => mi.Id).Should().HaveCount(2)
                .And.Contain(new[] { "11", "13" });
        }

        [Fact]
        public void GivenIncludedChildTagAndExcludedParentTag_NoMediaAreReturned()
        {
            string[] includedTags = { "usa" };
            string[] excludedTags = { "politics" };
            var searchMedia = new SearchMediaData()
            {
                IncludedTags = includedTags,
                ExcludedTags = excludedTags
            };
            var filteredMedia = GetFilteredMedia(searchMedia);

            filteredMedia.Should().BeEmpty();
        }

        private ReadOnlyCollection<MediaInstance> GetFilteredMedia(SearchMediaData searchMedia)
        {
            var mediaFilter = new MediaFilter(searchMedia.IncludedTags, searchMedia.ExcludedTags,
                searchMedia.TimeRangeStart, searchMedia.TimeRangeEnd, _tagGraph);
            var filteredMedia = mediaFilter.Filter(_context.MediaInstances.ToList().AsReadOnly());

            return filteredMedia;
        }

        //  books, islands, sport, university - not connected tags
        //
        //         politics     
        //         /       \
        //        /         \
        //     germany       usa     famous building
        //        \          /  \       /
        //         \        /    \     /
        //      schwarzenegger   white house
        //

        private void FillDatabase()
        {
            var tagNames = new[]
            {
                "books", "islands", "sport", "university",
                "politics", "usa", "germany", "famous building", "white house", "schwarzenegger"
            };
            foreach (var tagName in tagNames)
                _tagGraph.Add(new Tag() { Name = tagName });
            _tagGraph.CommitAllChanges();

            AddMediaInstance("1");
            AddMediaInstance("2");
            AddMediaInstance("3");

            AddMediaTag("1", "books");
            AddMediaTag("1", "islands");
            AddMediaTag("2", "islands");
            AddMediaTag("2", "sport");
            AddMediaTag("3", "university");

            _tagGraph["politics"].AddChild(_tagGraph["usa"]);
            _tagGraph["politics"].AddChild(_tagGraph["germany"]);
            _tagGraph["germany"].AddChild(_tagGraph["schwarzenegger"]);
            _tagGraph["usa"].AddChild(_tagGraph["schwarzenegger"]);
            _tagGraph["usa"].AddChild(_tagGraph["white house"]);
            _tagGraph["famous building"].AddChild(_tagGraph["white house"]);
            _tagGraph.CommitAllChanges();

            AddMediaInstance("11");
            AddMediaInstance("12");
            AddMediaInstance("13");
            AddMediaInstance("14");
            AddMediaInstance("15");
            AddMediaInstance("16");

            AddMediaTag("11", "politics");
            AddMediaTag("12", "usa");
            AddMediaTag("13", "germany");
            AddMediaTag("14", "schwarzenegger");
            AddMediaTag("15", "famous building");
            AddMediaTag("16", "white house");
        }

        private void AddMediaInstance(string id)
        {
            var newMediaInstance = new MediaInstance()
            {
                Id = id,
                DataType = "png"
            };

            _context.MediaInstances.Add(newMediaInstance);
            _context.SaveChanges();
        }

        private void AddMediaTag(string mediaInstanceId, string tagName)
        {
            var mediaInstance = _context.MediaInstances.Single(mi => mi.Id == mediaInstanceId);
            var tag = _context.Tags.Single(t => t.Name == tagName);

            var mediaTag = new MediaTag
            {
                MediaInstance = mediaInstance,
                MediaInstanceId = mediaInstance.Id,
                Tag = tag,
                TagId = tag.Id
            };

            mediaInstance.MediaTags.Add(mediaTag);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
