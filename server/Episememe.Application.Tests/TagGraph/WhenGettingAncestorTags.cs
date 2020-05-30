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
    public class WhenGettingAncestorTags : IDisposable
    {
        private readonly DbConnection _dbConnection;
        private readonly IWritableApplicationContext _givenContext;

        public WhenGettingAncestorTags()
        {
            (_givenContext, _dbConnection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _givenContext.Tags.AddRange(_givenTags);
            _givenContext.TagConnections.AddRange(_givenTagConnections);
            _givenContext.SaveChanges();
        }

        [Fact]
        public void GivenRootTag_ThenReturnsEmptyCollection()
        {
            var sut = new TagGraphService(_givenContext);

            var ancestors = sut["Root"].AncestorTags;

            ancestors.Should().BeEmpty();
        }

        [Fact]
        public void Given2LevelsDeepTag_ThenReturnsDirectParent()
        {
            var sut = new TagGraphService(_givenContext);

            var ancestors = sut["2LevelsDeep"].AncestorTags;

            var ancestorNames = ancestors.Select(t => t.Name);
            ancestorNames.Should().Contain("1LevelDeep");
        }

        [Fact]
        public void Given2LevelsDeepTag_ThenReturnsIndirectAncestor()
        {
            var sut = new TagGraphService(_givenContext);

            var ancestors = sut["2LevelsDeep"].AncestorTags;

            var ancestorNames = ancestors.Select(t => t.Name);
            ancestorNames.Should().Contain("Root");
        }

        [Fact]
        public void Given2LevelsDeepTag_ThenDoesNotReturnInvalidTags()
        {
            var sut = new TagGraphService(_givenContext);

            var ancestors = sut["2LevelsDeep"].AncestorTags;

            var ancestorNames = ancestors.Select(t => t.Name);
            ancestorNames.Should().NotContain(new[] { "Another1LevelDeep", "3LevelsDeep" });
        }


        [Fact]
        public void Given2LevelsDeepTag_ThenReturnsDistinctValues()
        {
            var sut = new TagGraphService(_givenContext);

            var ancestors = sut["2LevelsDeep"].AncestorTags;

            ancestors.Should().OnlyHaveUniqueItems(tag => tag.Name);
        }

        /*          Root
         *          ^  ^
         * 1LevelDeep  Another1LevelDeep
         *      ^
         * 2LevelsDeep
         *      ^
         * 3LevelsDeep
         */

        private readonly Tag[] _givenTags =
        {
            new Tag {Id = 1, Name = "Root"},
            new Tag {Id = 2, Name = "1LevelDeep"},
            new Tag {Id = 3, Name = "2LevelsDeep"},
            new Tag {Id = 4, Name = "Another1LevelDeep"},
            new Tag {Id = 5, Name = "3LevelsDeep"}
        };

        private readonly TagConnection[] _givenTagConnections =
        {
            new TagConnection {SuccessorId = 2, AncestorId = 1, Depth = 1},
            new TagConnection {SuccessorId = 4, AncestorId = 1, Depth = 1},
            new TagConnection {SuccessorId = 3, AncestorId = 2, Depth = 1},
            new TagConnection {SuccessorId = 3, AncestorId = 1, Depth = 2},
            new TagConnection {SuccessorId = 5, AncestorId = 4, Depth = 1},
            new TagConnection {SuccessorId = 5, AncestorId = 2, Depth = 2},
            new TagConnection {SuccessorId = 5, AncestorId = 1, Depth = 3}
        };

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}
