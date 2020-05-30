using System;
using System.Data.Common;
using Episememe.Application.Interfaces;
using Episememe.Application.TagGraph;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using FluentAssertions;
using Xunit;

namespace Episememe.Application.Tests.TagGraph
{
    public class WhenCreatingTagVertex : IDisposable
    {
        private readonly DbConnection _dbConnection;
        private readonly IWritableApplicationContext _givenContext;

        public WhenCreatingTagVertex()
        {
            (_givenContext, _dbConnection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _givenContext.Tags.AddRange(_givenTags);
            _givenContext.TagConnections.AddRange(_givenTagConnections);
            _givenContext.SaveChanges();
        }

        [Fact]
        public void GivenNonexistentTag_ThenThrows()
        {
            var sut = new TagGraphService(_givenContext);

            Action act = () => _ = sut["Nonexistent tag"];

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void GivenExistingTag_ThenReturnsTagVertex()
        {
            var sut = new TagGraphService(_givenContext);

            var tagVertex = sut["Existing tag"];

            tagVertex.Should().NotBeNull();
        }

        private readonly Tag[] _givenTags =
        {
            new Tag {Id = 1, Name = "Existing tag"}
        };

        private readonly TagConnection[] _givenTagConnections = Array.Empty<TagConnection>();

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}
