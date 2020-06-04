using System.Data.Common;
using System.Linq;
using Episememe.Application.Interfaces;
using Episememe.Application.TagGraph;
using Episememe.Application.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace Episememe.Application.Tests.TagGraph.Scenarios
{
    /* Tested graph:
     *  2  4
     *  ⭦ ⭧
     *   0
     *  ⭧ ⭦
     *  3  5
     *  ⭦ ⭧
     *   7
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
        public void NonZeroEdgeCount()
        {
            _context.TagConnections.Count().Should().BeGreaterThan(0);
        }

        private void ConstructGraph()
        {
            _sut.Connect(0, 2);
            _sut.Connect(3, 0);
            _sut.Connect(7, 5);
            _sut.Connect(5, 0);
            _sut.Connect(7, 3);
            _sut.Connect(0, 4);
            _sut.SaveChanges();
        }
    }
}
