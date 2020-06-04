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
        private readonly string[] _upperGraphNames = { "1", "2", "3", "4" };
        private readonly string[] _lowerGraphNames = { "0", "5", "6", "7" };

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
        public void UpperVerticesHaveNoSuccessorsFromLowerGraph()
        {
            var upperSuccessors = _upperGraphNames
                .SelectMany(name => _sut[name].Successors);

            upperSuccessors.Should().NotContain(tag => _lowerGraphNames.Contains(tag.Name));
        }

        [Fact]
        public void LowerVerticesHaveNoAncestorsFromUpperGraph()
        {
            var lowerAncestors = _lowerGraphNames
                .SelectMany(name => _sut[name].Ancestors);

            lowerAncestors.Should().NotContain(tag => _upperGraphNames.Contains(tag.Name));
        }

        private void ConstructGraph()
        {
            foreach (var name in new[] {"0", "1", "2", "3", "4", "5", "6", "7"})
                _sut.Add(new Tag {Name = name});
            _sut.SaveChanges();

            _sut["0"].AddParent(_sut["1"]);
            _sut["1"].AddParent(_sut["2"]);
            _sut["1"].AddParent(_sut["3"]);
            _sut["3"].AddParent(_sut["4"]);

            _sut["5"].AddParent(_sut["0"]);
            _sut["6"].AddParent(_sut["5"]);
            _sut["7"].AddParent(_sut["5"]);
            _sut.SaveChanges();

            _sut["0"].DeleteParent(_sut["1"]);
            _sut.SaveChanges();
        }
    }
}
