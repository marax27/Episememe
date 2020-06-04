using System.Data.Common;
using Episememe.Application.Interfaces;
using Episememe.Application.TagGraph;
using Episememe.Application.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace Episememe.Application.Tests.TagGraph.Scenarios
{
    /* Tested graph:
     * 0 ⭢ 1 ⭢ 2 ⭢ 3 ⭢ 4 ⭢ 5
     */
    public class LinkedListTest
    {
        private readonly IWritableApplicationContext _context;
        private readonly DbConnection _connection;
        private readonly ITagGraphService _sut;

        public LinkedListTest()
        {
            (_context, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _sut = new TagGraphService(_context);
            ConstructGraph();
        }

        [Fact]
        public void TopVertexHasExpectedSuccessors()
        {
            _sut.Successors(5).Should().BeEquivalentTo(0, 1, 2, 3, 4);
        }

        [Fact]
        public void TopVertexHasNoAncestors()
        {
            _sut.Ancestors(5).Should().BeEmpty();
        }

        [Fact]
        public void BottomVertexHasNoSuccessors()
        {
            _sut.Successors(0).Should().BeEmpty();
        }

        [Fact]
        public void BottomVertexHasExpectedAncestors()
        {
            _sut.Ancestors(0).Should().BeEquivalentTo(1, 2, 3, 4, 5);
        }

        [Fact]
        public void MiddleVertexHasExpectedSuccessors()
        {
            _sut.Successors(3).Should().BeEquivalentTo(0, 1, 2);
        }

        [Fact]
        public void MiddleVertexHasExpectedAncestors()
        {
            _sut.Ancestors(3).Should().BeEquivalentTo(4, 5);
        }

        private void ConstructGraph()
        {
            _sut.Connect(2, 3);
            _sut.Connect(0, 1);
            _sut.Connect(3, 4);
            _sut.Connect(4, 5);
            _sut.Connect(1, 2);
            _sut.SaveChanges();
        }
    }
}
