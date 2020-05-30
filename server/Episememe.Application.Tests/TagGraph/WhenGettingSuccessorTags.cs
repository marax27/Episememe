using System;
using System.Data.Common;
using System.Linq;
using Episememe.Application.Interfaces;
using Episememe.Application.TagGraph;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using FluentAssertions;
using Xunit;

namespace Episememe.Application.Tests.TagGraph
{
    public class WhenGettingSuccessorTags : IDisposable
    {
        private readonly DbConnection _dbConnection;
        private readonly IWritableApplicationContext _givenContext;

        public WhenGettingSuccessorTags()
        {
            (_givenContext, _dbConnection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _givenContext.Tags.AddRange(_givenTags);
            _givenContext.TagConnections.AddRange(_givenTagConnections);
            _givenContext.SaveChanges();
        }

        [Fact]
        public void GivenRootTag_ThenRootIsNotReturned()
        {
            var sut = new TagGraphService(_givenContext);

            var successors = sut["Root"].SuccessorTags;

            successors.Should().NotContain(tag => tag.Name == "Root");
        }

        [Fact]
        public void GivenRootTag_ThenReturnsDirectChildren()
        {
            var sut = new TagGraphService(_givenContext);

            var successors = sut["Root"].SuccessorTags;

            var successorNames = successors.Select(t => t.Name);
            successorNames.Should().Contain(new[] {"1LevelDeep", "Another1LevelDeep"});
        }

        [Fact]
        public void GivenRootTag_ThenReturnsIndirectSuccessors()
        {
            var sut = new TagGraphService(_givenContext);

            var successors = sut["Root"].SuccessorTags;

            var successorNames = successors.Select(t => t.Name);
            successorNames.Should().Contain("2LevelsDeep");
        }

        [Fact]
        public void GivenRootTag_ThenReturnsDistinctValues()
        {
            var sut = new TagGraphService(_givenContext);

            var successors = sut["Root"].SuccessorTags;

            successors.Should().OnlyHaveUniqueItems(tag => tag.Name);
        }

        [Fact]
        public void GivenTagWithoutSuccessors_ThenReturnsEmptyCollection()
        {
            var sut = new  TagGraphService(_givenContext);

            var successors = sut["2LevelsDeep"].SuccessorTags;

            successors.Should().BeEmpty();
        }

        /*          Root
         *          ^  ^
         *          |  |
         * 1LevelDeep  Another1LevelDeep
         *      ^
         *      |
         * 2LevelsDeep
         */

        private readonly Tag[] _givenTags =
        {
            new Tag {Id = 1, Name = "Root"},
            new Tag {Id = 2, Name = "1LevelDeep"},
            new Tag {Id = 3, Name = "2LevelsDeep"},
            new Tag {Id = 4, Name = "Another1LevelDeep"}
        };

        private readonly TagConnection[] _givenTagConnections =
        {
            new TagConnection {SuccessorId = 2, AncestorId = 1, Depth = 1},
            new TagConnection {SuccessorId = 4, AncestorId = 1, Depth = 1},
            new TagConnection {SuccessorId = 3, AncestorId = 2, Depth = 1},
            new TagConnection {SuccessorId = 3, AncestorId = 1, Depth = 2}
        };

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}
