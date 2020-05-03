using System;
using System.Collections.Generic;
using System.Threading;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Application.DataTransfer;
using Episememe.Application.Features.GetTags;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Newtonsoft.Json;

namespace Episememe.Application.Tests.FeatureTests.GetTags
{
    public class WhenGettingTags{

        private readonly Mock<DbSet<Tag>> _browseTokensMock;
        
        [Fact]
        public void GetExistingTags(){
            IApplicationContext context = CreateMockApplicationContext();

            var query = GetTagsQuery.Create();
            IRequestHandler<GetTagsQuery, IEnumerable<TagInstanceDto>> sut = new GetTagsQueryHandler(context);

            var actualResult = sut.Handle(query, CancellationToken.None).Result;
            var expectedResult = CreateExpectedResult();

            var obj1Str = JsonConvert.SerializeObject(actualResult);
            var obj2Str = JsonConvert.SerializeObject(expectedResult);

            Assert.Equal(obj1Str, obj2Str);
        }

        private IApplicationContext CreateMockApplicationContext()
        {
            var givenTags = new[] { new Tag{Id = 52, Name = "random", Description = "It is so random"}, new Tag{Id = 62, Name = "expected", Description = "Spanish Inquisition"} };
            var tagMock = DbSetMockFactory.Create(givenTags);

            var mock = new Mock<IApplicationContext>();
            mock.Setup(m => m.Tags).Returns(tagMock.Object);

            return mock.Object;
        }

        private IEnumerable<TagInstanceDto> CreateExpectedResult(){
            List<TagInstanceDto> list = new List<TagInstanceDto>();
            list.Add(new TagInstanceDto("random", "It is so random"));
            list.Add(new TagInstanceDto("expected", "Spanish Inquisition"));
            IEnumerable<TagInstanceDto> expected = list;
            return expected;
        }
    }
}