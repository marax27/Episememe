using System;
using System.Data.Common;
using Episememe.Application.Graphs.Exceptions;
using Episememe.Application.Graphs.Interfaces;
using Episememe.Application.Graphs.Tags;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Episememe.Application.Tests.TagGraphTests
{
    public class WhenCreatingCycle
    {
        private readonly IWritableApplicationContext _context;
        private readonly DbConnection _connection;
        private readonly IGraph<Tag> _sut;

        public WhenCreatingCycle()
        {
            (_context, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
            _sut = new TagGraph(_context);
        }

        [Fact]
        public void GivenSelfReferencingVertex_ThenThrowsCycleException()
        {
            var tag = _sut.Add(new Tag {Name = "Sample tag"});
            _sut.SaveChanges();

            Action act = () => tag.AddParent(tag);

            act.Should().Throw<CycleException>();
        }

        [Fact]
        public void GivenTwoVertexCycle_ThenThrowsCycleException()
        {
            var aTag = _sut.Add(new Tag {Name = "A"});
            var bTag = _sut.Add(new Tag {Name = "B"});
            _sut.SaveChanges();
            aTag.AddParent(bTag);
            _sut.SaveChanges();

            Action act = () => aTag.AddChild(bTag);

            act.Should().Throw<CycleException>();
        }

        [Fact]
        public void GivenThreeVertexCycle_ThenThrowsCycleException()
        {
            var aTag = _sut.Add(new Tag { Name = "A" });
            var bTag = _sut.Add(new Tag { Name = "B" });
            var cTag = _sut.Add(new Tag { Name = "C" });
            _sut.SaveChanges();
            aTag.AddParent(bTag);
            bTag.AddParent(cTag);
            _sut.SaveChanges();

            Action act = () => cTag.AddParent(aTag);

            act.Should().Throw<CycleException>();
        }
    }
}
