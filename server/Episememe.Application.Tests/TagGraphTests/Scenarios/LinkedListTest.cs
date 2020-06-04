using System.Data.Common;
using System.Linq;
using Episememe.Application.Graphs.Interfaces;
using Episememe.Application.Graphs.Tags;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Episememe.Application.Tests.TagGraphTests.Scenarios
{
    /* Tested graph:
     * 0 ⭢ 1 ⭢ 2 ⭢ 3 ⭢ 4 ⭢ 5
     */
    public class LinkedListTest
    {
        private readonly IWritableApplicationContext _context;
        private readonly DbConnection _connection;
        private readonly IGraph<Tag> _sut;

        public LinkedListTest()
        {
            (_context, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _sut = new TagGraph(_context);
            ConstructGraph();
        }

        [Fact]
        public void TopVertexHasExpectedSuccessors()
        {
            var actualSuccessorNames = _sut["5"].Successors
                .Select(tag => tag.Name);
            actualSuccessorNames.Should().BeEquivalentTo("0", "1", "2", "3", "4");
        }

        [Fact]
        public void TopVertexHasNoAncestors()
        {
            var actualAncestorNames = _sut["5"].Ancestors
                .Select(tag => tag.Name);
            actualAncestorNames.Should().BeEmpty();
        }

        [Fact]
        public void BottomVertexHasNoSuccessors()
        {
            var actualSuccessorNames = _sut["0"].Successors
                .Select(tag => tag.Name);
            actualSuccessorNames.Should().BeEmpty();
        }

        [Fact]
        public void BottomVertexHasExpectedAncestors()
        {
            var actualAncestorNames = _sut["0"].Ancestors
                .Select(tag => tag.Name);
            actualAncestorNames.Should().BeEquivalentTo("1", "2", "3", "4", "5");
        }

        [Fact]
        public void MiddleVertexHasExpectedSuccessors()
        {
            var actualSuccessorNames = _sut["3"].Successors
                .Select(tag => tag.Name);
            actualSuccessorNames.Should().BeEquivalentTo("0", "1", "2");
        }

        [Fact]
        public void MiddleVertexHasExpectedAncestors()
        {
            var actualAncestorNames = _sut["3"].Ancestors
                .Select(tag => tag.Name);
            actualAncestorNames.Should().BeEquivalentTo("4", "5");
        }

        private void ConstructGraph()
        {
            _sut.Add(new Tag {Name = "0"});
            _sut.Add(new Tag {Name = "1"});
            _sut.Add(new Tag {Name = "2"});
            _sut.Add(new Tag {Name = "3"});
            _sut.Add(new Tag {Name = "4"});
            _sut.Add(new Tag {Name = "5"});
            _sut.SaveChanges();

            _sut["2"].AddParent(_sut["3"]);
            _sut["0"].AddParent(_sut["1"]);
            _sut["3"].AddParent(_sut["4"]);
            _sut["4"].AddParent(_sut["5"]);
            _sut["1"].AddParent(_sut["2"]);
            _sut.SaveChanges();
        }
    }
}
