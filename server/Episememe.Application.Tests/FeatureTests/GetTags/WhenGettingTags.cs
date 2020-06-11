using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Episememe.Domain.Entities;
using Episememe.Application.DataTransfer;
using Episememe.Application.Features.GetTags;
using Episememe.Application.Graphs.Interfaces;
using MediatR;
using Moq;
using Xunit;
using FluentAssertions;

namespace Episememe.Application.Tests.FeatureTests.GetTags
{
    public class WhenGettingTags
    {
        [Fact]
        public void GivenContextWithSampleTags_ThenAllTagsAreReturned()
        {
            var givenTags = new[] { new Tag{Id = 52, Name = "random", Description = "It is so random"}, new Tag{Id = 62, Name = "expected", Description = "Spanish Inquisition"} };
            var expectedResult = new[] 
            {
                new TagInstanceDto("random", "It is so random", Array.Empty<string>(), Array.Empty<string>()), 
                new TagInstanceDto("expected", "Spanish Inquisition", Array.Empty<string>(), Array.Empty<string>())
            };
            var mockGraph = CreateMockTagGraph(givenTags);

            var query = GetTagsQuery.Create();
            IRequestHandler<GetTagsQuery, IEnumerable<TagInstanceDto>> sut = new GetTagsQueryHandler(mockGraph.Object);
            var actualResult = sut.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GivenContextWithoutTags_NoTagsAreReturned()
        {
            var givenTags = new List<Tag>();
            var expectedResult = new List<TagInstanceDto>();
            var mockGraph = CreateMockTagGraph(givenTags);
            
            var query = GetTagsQuery.Create();
            IRequestHandler<GetTagsQuery, IEnumerable<TagInstanceDto>> sut = new GetTagsQueryHandler(mockGraph.Object);
            var actualResult = sut.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private Mock<IGraph<Tag>> CreateMockTagGraph(IEnumerable<Tag> givenTags)
        {
            var mockGraphs = new Mock<IGraph<Tag>>();
            var mockVertices = givenTags.Select(tag =>
            {
                var tagMock = new Mock<IVertex<Tag>>();
                tagMock.Setup(m => m.Entity).Returns(tag);
                tagMock.Setup(m => m.Children).Returns(Array.Empty<Tag>());
                tagMock.Setup(m => m.Parents).Returns(Array.Empty<Tag>());
                return tagMock.Object;
            });

            mockGraphs.Setup(m => m.Vertices)
                .Returns(mockVertices);
            return mockGraphs;
        }
    }
}