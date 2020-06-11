using Episememe.Application.Features.SearchMedia;
using Episememe.Application.Filtering.BaseFiltering;
using Episememe.Application.Graphs.Interfaces;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Episememe.Application.Tests.Filtering
{
    public class WhenFilteringMediaByTags
    {
        private readonly ICollection<MediaInstance> _mediaInstances;
        private readonly Mock<IGraph<Tag>> _tagGraph;

        public WhenFilteringMediaByTags()
        {
            _mediaInstances = new List<MediaInstance>();
            _tagGraph = new Mock<IGraph<Tag>>();
            SetupMocks();
        }

        [Fact]
        public void GivenNullFilterArguments_AllMediaAreReturned()
        {
            var filteredMedia = GetFilteredMedia(new SearchMediaData());

            filteredMedia.Should().BeEquivalentTo(_mediaInstances);
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

            filteredMedia.Should().BeEquivalentTo(_mediaInstances);
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
                searchMedia.TimeRangeStart, searchMedia.TimeRangeEnd, _tagGraph.Object);
            var filteredMedia = mediaFilter.Filter(_mediaInstances.ToList().AsReadOnly());

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

        private void SetupMocks()
        {
            var tagId = 0;
            var tagsDict = new[]
            {
                "books", "islands", "sport", "university",
                "politics", "usa", "germany", "famous building", "white house", "schwarzenegger"
            }.Select(name => new Tag { Id = ++tagId, Name = name })
                .ToDictionary(t => t.Name);

            var mediaInstancesDict = new List<MediaInstance>(
                new[] { "1", "2", "3", "11", "12", "13", "14", "15", "16" }.Select(CreateMediaInstance)
            ).ToDictionary(mi => mi.Id);

            CreateTagGraph(tagsDict);
            CreateMediaTags(mediaInstancesDict, tagsDict);

            foreach (var mediaInstance in mediaInstancesDict.Values)
                _mediaInstances.Add(mediaInstance);
        }

        private void CreateTagGraph(Dictionary<string, Tag> tags)
        {
            var tagGraphVertices = new[]
            {
                (tags["books"], Array.Empty<Tag>(), Array.Empty<Tag>()),
                (tags["islands"], Array.Empty<Tag>(), Array.Empty<Tag>()),
                (tags["sport"], Array.Empty<Tag>(), Array.Empty<Tag>()),
                (tags["university"], Array.Empty<Tag>(), Array.Empty<Tag>()),
                (tags["politics"], Array.Empty<Tag>(),
                    new[] {tags["germany"], tags["usa"], tags["schwarzenegger"], tags["white house"]}),
                (tags["germany"], new[] {tags["politics"]}, new[] {tags["schwarzenegger"]}),
                (tags["usa"], new[] {tags["politics"]}, new[] {tags["schwarzenegger"], tags["white house"]}),
                (tags["schwarzenegger"], new[] {tags["politics"], tags["germany"], tags["usa"]}, Array.Empty<Tag>()),
                (tags["famous building"], Array.Empty<Tag>(), new[] {tags["white house"]}),
                (tags["white house"], new[] {tags["politics"], tags["usa"], tags["famous building"]}, Array.Empty<Tag>())
            }.Select(args => CreateTagVertex(args.Item1, args.Item2, args.Item3)).ToList();

            _tagGraph.Setup(graph => graph.Vertices).Returns(tagGraphVertices);
            _tagGraph.Setup(graph => graph[It.IsAny<string>()])
                .Returns((string key) => tagGraphVertices.Single(tv => tv.Entity.Name == key));
        }

        private void CreateMediaTags(Dictionary<string, MediaInstance> mediaInstances, Dictionary<string, Tag> tags)
        {
            var pairsToConnect = new[] { ("1", "books"), ("1", "islands"), ("2", "islands"), ("2", "sport"),
                ("3", "university"), ("11", "politics"), ("12", "usa"), ("13", "germany"), ("14", "schwarzenegger"),
                ("15", "famous building"), ("16", "white house") };

            foreach (var (mediaInstanceId, tagName) in pairsToConnect)
                CreateMediaTag(mediaInstances[mediaInstanceId], tags[tagName]);
        }

        private IVertex<Tag> CreateTagVertex(Tag tag, IEnumerable<Tag> ancestors, IEnumerable<Tag> successors)
        {
            var tagMock = new Mock<IVertex<Tag>>();
            tagMock.Setup(m => m.Entity).Returns(tag);
            tagMock.Setup(m => m.Ancestors).Returns(ancestors);
            tagMock.Setup(m => m.Successors).Returns(successors);

            return tagMock.Object;
        }

        private MediaInstance CreateMediaInstance(string id)
        {
            var newMediaInstance = new MediaInstance()
            {
                Id = id,
                DataType = "png"
            };

            return newMediaInstance;
        }

        private void CreateMediaTag(MediaInstance mediaInstance, Tag tag)
        {
            var mediaTag = new MediaTag
            {
                MediaInstance = mediaInstance,
                MediaInstanceId = mediaInstance.Id,
                Tag = tag,
                TagId = tag.Id
            };

            mediaInstance.MediaTags.Add(mediaTag);
            tag.MediaTags.Add(mediaTag);
        }
    }
}
