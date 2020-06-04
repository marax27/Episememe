using System.Data.Common;
using System.Linq;
using Episememe.Application.Interfaces;
using Episememe.Application.TagGraph;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Episememe.Application.Tests.TagGraph.Scenarios
{
    /* Tested graph:
     *  U1  U2
     *   ⭦ ⭧
     *    S1
     *   ⭧ ⭦
     *  A1  A2
     *   ⭦ ⭧
     *    B1
     */
    public class TwoSymmetricalPathsTest
    {
        private readonly IWritableApplicationContext _context;
        private readonly DbConnection _connection;
        private readonly ITagGraphService _sut;

        public TwoSymmetricalPathsTest()
        {
            (_context, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _sut = new TagGraphService(_context);
            ConstructGraph();
        }

        [Fact]
        public void S1HasExpectedSuccessors()
        {
            var actualSuccessorNames = _sut["S1"].Successors
                .Select(tag => tag.Name);
            actualSuccessorNames.Should().BeEquivalentTo("A1", "A2", "B1");
        }

        [Fact]
        public void S1HasExpectedAncestors()
        {
            var actualAncestorNames = _sut["S1"].Ancestors
                .Select(tag => tag.Name);
            actualAncestorNames.Should().BeEquivalentTo("U1", "U2");
        }

        [Fact]
        public void B1HasNoSuccessors()
        {
            _sut["B1"].Successors.Should().BeEmpty();
        }

        [Fact]
        public void B1HasExpectedAncestors()
        {
            var actualSuccessorNames = _sut["B1"].Ancestors
                .Select(tag => tag.Name);
            actualSuccessorNames.Should().BeEquivalentTo("A1", "A2", "S1", "U1", "U2");
        }

        private void ConstructGraph()
        {
            var u1 = _sut.Add(new Tag {Name = "U1"});
            var u2 = _sut.Add(new Tag {Name = "U2"});
            var s1 = _sut.Add(new Tag {Name = "S1"});
            var a1 = _sut.Add(new Tag {Name = "A1"});
            var a2 = _sut.Add(new Tag {Name = "A2"});
            var b1 = _sut.Add(new Tag {Name = "B1"});
            _sut.SaveChanges();

            s1.AddParent(u1);
            a1.AddParent(s1);
            b1.AddParent(a2);
            a2.AddParent(s1);
            b1.AddParent(a1);
            s1.AddParent(u2);

            _sut.SaveChanges();
        }
    }
}
