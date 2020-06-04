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
     *      4
     *      ⭡
     * 2    3
     *  ⭦ ⭧
     *   1
     *   ⭡ -- edge that will be removed
     *   0
     *   ⭡
     *   5
     *  ⭧ ⭦
     * 6    7
     */
    public class SplitInto2GraphsTest
    {
        private readonly IWritableApplicationContext _context;
        private readonly DbConnection _connection;
        private readonly ITagGraphService _sut;

        public SplitInto2GraphsTest()
        {
            (_context, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _sut = new TagGraphService(_context);
            ConstructGraph();
        }

        [Fact]
        public void GraphsAreDisconnected()
        {
            var upperGraphIds = new[] {1, 2, 3, 4};
            var lowerGraphIds = new[] {0, 5, 6, 7};

            _context.TagConnections.Should()
                .NotContain(tc => upperGraphIds.Contains(tc.Ancestor)
                                  && lowerGraphIds.Contains(tc.Successor));
        }

        private void ConstructGraph()
        {
            _sut.Connect(0, 1);
            _sut.Connect(1, 2);
            _sut.Connect(1, 3);
            _sut.Connect(3, 4);

            _sut.Connect(5, 0);
            _sut.Connect(6, 5);
            _sut.Connect(7, 5);
            _sut.SaveChanges();

            _sut.Disconnect(0, 1);
            _sut.SaveChanges();
        }
    }
}
