using System;
using System.Data.Common;
using Episememe.Application.Interfaces;
using Episememe.Application.TagGraph;
using Episememe.Application.TagGraph.Exceptions;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using FluentAssertions;
using Xunit;

namespace Episememe.Application.Tests.TagGraph
{
    public class WhenAddingParentWouldCreateCycle : IDisposable
    {
        private readonly DbConnection _dbConnection;
        private readonly IWritableApplicationContext _givenContext;

        public WhenAddingParentWouldCreateCycle()
        {
            (_givenContext, _dbConnection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _givenContext.Tags.AddRange(_givenTags);
            _givenContext.TagConnections.AddRange(_givenTagConnections);
            _givenContext.SaveChanges();
        }

        [Fact]
        public void GivenTopToBottomConnection_ThenThrowsCycleException()
        {
            var sut = new TagGraphService(_givenContext);

            Action act = () =>
            {
                sut["Top"].AddParent(sut["Bottom"]);
                sut.SaveChanges();
            };

            act.Should().Throw<CycleException>();
        }

        [Fact]
        public void GivenAbcCycle_ThenThrowsCycleException()
        {
            var sut = new TagGraphService(_givenContext);

            Action act = () =>
            {
                sut["C"].AddParent(sut["A"]);
                sut.SaveChanges();
            };

            act.Should().Throw<CycleException>();
        }


        /*
         *  Top           C
         *   ^            ^
         * Bottom    A -> B 
         *
         */

        private readonly Tag[] _givenTags =
        {
            new Tag {Id = 1, Name = "Top"},
            new Tag {Id = 2, Name = "Bottom"},
            new Tag {Id = 3, Name = "A"},
            new Tag {Id = 4, Name = "B"},
            new Tag {Id = 5, Name = "C"}
        };

        private readonly TagConnection[] _givenTagConnections =
        {
            new TagConnection {SuccessorId = 2, AncestorId = 1, Depth = 1},
            new TagConnection {SuccessorId = 4, AncestorId = 5, Depth = 1},
            new TagConnection {SuccessorId = 3, AncestorId = 4, Depth = 1},
            new TagConnection {SuccessorId = 3, AncestorId = 5, Depth = 2}
        };

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}
