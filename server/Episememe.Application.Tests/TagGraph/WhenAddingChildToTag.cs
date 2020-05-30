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
    public class WhenAddingChildToTag : IDisposable
    {
        private readonly DbConnection _dbConnection;
        private readonly IWritableApplicationContext _givenContext;

        public WhenAddingChildToTag()
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

            sut["Main"].AddChild(sut["NewChild"]);
            sut.SaveChanges();

            _givenContext.TagConnections.Should()
                .Contain(connection =>
                    connection.Successor.Name == "NewChild"
                    && connection.Ancestor.Name == "Main"
                    && connection.Depth == 1
                );
        }

        [Fact]
        public void GivenSampleTags_DirectConnectionIsCreatedExactlyOnce()
        {
            var sut = new TagGraphService(_givenContext);

            sut["Main"].AddChild(sut["NewChild"]);
            sut.SaveChanges();

            _givenContext.TagConnections.Should()
                .ContainSingle(connection =>
                    connection.Successor.Name == "NewChild"
                    && connection.Ancestor.Name == "Main"
                    && connection.Depth == 1
                );
        }

        [Fact]
        public void GivenSampleTags_IndirectConnectionIsCreated()
        {
            var sut = new TagGraphService(_givenContext);

            sut["Main"].AddChild(sut["NewChild"]);
            sut.SaveChanges();

            _givenContext.TagConnections.Should()
                .Contain(connection =>
                    connection.Successor.Name == "NewChild"
                    && connection.Ancestor.Name == "Parent"
                    && connection.Depth == 2);
        }

        /*
         * Parent
         *   ^
         * Main
         *   ^
         * Child   NewChild
         */

        private readonly Tag[] _givenTags =
        {
            new Tag {Id = 1, Name = "Parent"},
            new Tag {Id = 2, Name = "Main"},
            new Tag {Id = 3, Name = "Child"},
            new Tag {Id = 4, Name = "NewChild"}
        };

        private readonly TagConnection[] _givenTagConnections =
        {
            new TagConnection {SuccessorId = 3, AncestorId = 2, Depth = 1},
            new TagConnection {SuccessorId = 2, AncestorId = 1, Depth = 1},
            new TagConnection {SuccessorId = 3, AncestorId = 1, Depth = 2}
        };

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}
