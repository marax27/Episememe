using System;
using System.Data.Common;
using Episememe.Application.Interfaces;
using Episememe.Application.TagGraph;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Episememe.Application.Tests.TagGraph
{
    public class WhenCreatingNewTag : IDisposable
    {
        private readonly DbConnection _dbConnection;
        private readonly IWritableApplicationContext _givenContext;
        private readonly ITagGraphService _sut;

        public WhenCreatingNewTag()
        {
            (_givenContext, _dbConnection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _givenContext.Tags.AddRange(_givenTags);
            _givenContext.SaveChanges();
            _sut = new TagGraphService(_givenContext);
        }

        [Fact]
        public void GivenSampleTag_ThenCreatesTag()
        {
            var givenTag = new Tag {Name = "New Tag", Description = "1234"};

            _sut.Create(givenTag);
            _sut.SaveChanges();

            _givenContext.Tags.Should()
                .Contain(tag => tag.Name == "New Tag" && tag.Description == "1234");
        }

        [Fact]
        public void GivenTagNameAlreadyExists_ThenThrows()
        {
            var givenTag = new Tag {Name = "Old tag name", Description = "abc"};

            Action act = () =>
            {
                _sut.Create(givenTag);
                _sut.SaveChanges();
            };

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void GivenNullTag_ThenThrows()
        {
            Action act = () =>
            {
                _sut.Create(null);
                _sut.SaveChanges();
            };

            act.Should().Throw<Exception>();
        }

        private readonly Tag[] _givenTags =
        {
            new Tag {Name = "Old tag name", Description = "Example description"}
        };

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}
