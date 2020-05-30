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
    public class WhenAddingParentToTag : IDisposable
    {
        private readonly DbConnection _dbConnection;
        private readonly IWritableApplicationContext _givenContext;

        public WhenAddingParentToTag()
        {
            (_givenContext, _dbConnection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _givenContext.Tags.AddRange(_givenTags);
            _givenContext.TagConnections.AddRange(_givenTagConnections);
            _givenContext.SaveChanges();
        }

        [Fact]
        public void GivenSampleTags_DirectConnectionIsCreated()
        {
            var sut = new TagGraphService(_givenContext);

            sut["First"].AddParent(sut["Second"]);
            sut.SaveChanges();

            _givenContext.TagConnections.Should()
                .Contain(connection =>
                    connection.Successor.Name == "First"
                    && connection.Ancestor.Name == "Second"
                    && connection.Depth == 1
                );
        }

        [Fact]
        public void GivenSampleTags_DirectConnectionIsCreatedExactlyOnce()
        {
            var sut = new TagGraphService(_givenContext);

            sut["First"].AddParent(sut["Second"]);
            sut.SaveChanges();

            _givenContext.TagConnections.Should()
                .ContainSingle(connection =>
                    connection.Successor.Name == "First"
                    && connection.Ancestor.Name == "Second"
                    && connection.Depth == 1
                );
        }

        [Fact]
        public void GivenSampleTags_IndirectConnectionIsCreated()
        {
            var sut = new TagGraphService(_givenContext);

            sut["First"].AddParent(sut["Second"]);
            sut.SaveChanges();

            _givenContext.TagConnections.Should()
                .Contain(connection =>
                    connection.Successor.Name == "First"
                    && connection.Ancestor.Name == "Top"
                    && connection.Depth == 2);
        }

        /*
         *     Top
         *     ^ ^
         * First Second
         */

        private readonly Tag[] _givenTags =
        {
            new Tag {Id = 1, Name = "First"},
            new Tag {Id = 2, Name = "Second"},
            new Tag {Id = 3, Name = "Top"}
        };

        private readonly TagConnection[] _givenTagConnections =
        {
            new TagConnection {SuccessorId = 1, AncestorId = 3, Depth = 1},
            new TagConnection {SuccessorId = 2, AncestorId = 3, Depth = 1}
        };

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}
