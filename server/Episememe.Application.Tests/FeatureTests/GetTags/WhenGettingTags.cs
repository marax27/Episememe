using System.Collections.Generic;
using System.Threading;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Application.DataTransfer;
using Episememe.Application.Features.GetTags;
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
            var expectedResult = new[] {new TagInstanceDto("random", "It is so random"), new TagInstanceDto("expected", "Spanish Inquisition")};
            IApplicationContext context = CreateMockApplicationContext(givenTags);

            var query = GetTagsQuery.Create();
            IRequestHandler<GetTagsQuery, IEnumerable<TagInstanceDto>> sut = new GetTagsQueryHandler(context);
            var actualResult = sut.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GivenContextWithoutTags_NoTagsAreReturned()
        {
            var givenTags = new List<Tag>();
            var expectedResult = new List<TagInstanceDto>();
            IApplicationContext context = CreateMockApplicationContext(givenTags);
            
            var query = GetTagsQuery.Create();
            IRequestHandler<GetTagsQuery, IEnumerable<TagInstanceDto>> sut = new GetTagsQueryHandler(context);
            var actualResult = sut.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private IApplicationContext CreateMockApplicationContext(IEnumerable<Tag> givenTags)
        {
            var tagMock = DbSetMockFactory.Create(givenTags);

            var mock = new Mock<IApplicationContext>();
            mock.Setup(m => m.Tags).Returns(tagMock.Object);

            return mock.Object;
        }
    }
}