using System;
using System.Data.Common;
using System.Linq;
using Episememe.Application.Interfaces;
using Episememe.Application.TagGraph;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using FluentAssertions;
using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;

namespace Episememe.Application.Tests.TagGraph.Scenarios
{
    public class CreatingSampleTagGraph
    {
        private DbConnection _dbConnection;
        private IWritableApplicationContext _givenContext;
        private ITagGraphService _sut;

        [BddfyFact]
        public void Fact()
        {
            this.BDDfy();
        }

        private void GivenEmptyApplicationContext()
        {
            (_givenContext, _dbConnection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
        }

        private void AndGivenTagGraphService()
        {
            _sut = new TagGraphService(_givenContext);
        }

        private void WhenSampleGraphIsCreated()
        {
            Act();
        }

        private void ThenDatabaseContains4Tags()
        {
            _givenContext.Tags.Should().HaveCount(4);
        }


        private void AndRootHasExpectedSuccessors()
        {
            var actualSuccessorNames = _sut["Root"].SuccessorTags.Select(tag => tag.Name);
            actualSuccessorNames.Should().BeEquivalentTo("Left", "Middle", "Right");
        }

        private void AndRootHasExpectedAncestors()
        {
            var actualAncestorNames = _sut["Root"].AncestorTags.Select(tag => tag.Name);
            actualAncestorNames.Should().BeEquivalentTo(Array.Empty<string>());
        }

        private void AndLeftHasExpectedSuccessors()
        {
            var actualSuccessorNames = _sut["Left"].SuccessorTags.Select(tag => tag.Name);
            actualSuccessorNames.Should().BeEquivalentTo(Array.Empty<string>());
        }

        private void AndLeftHasExpectedAncestors()
        {
            var actualAncestorNames = _sut["Left"].AncestorTags.Select(tag => tag.Name);
            actualAncestorNames.Should().BeEquivalentTo("Right", "Middle", "Root");
        }

        private void AndRightHasExpectedSuccessors()
        {
            var actualSuccessorNames = _sut["Right"].SuccessorTags.Select(tag => tag.Name);
            actualSuccessorNames.Should().BeEquivalentTo("Left");
        }

        private void AndRightHasExpectedAncestors()
        {
            var actualAncestorNames = _sut["Right"].AncestorTags.Select(tag => tag.Name);
            actualAncestorNames.Should().BeEquivalentTo("Middle", "Root");
        }

        private void AndMiddleHasExpectedSuccessors()
        {
            var actualSuccessorNames = _sut["Middle"].SuccessorTags.Select(tag => tag.Name);
            actualSuccessorNames.Should().BeEquivalentTo("Left", "Right");
        }

        private void AndMiddleHasExpectedAncestors()
        {
            var actualAncestorNames = _sut["Middle"].AncestorTags.Select(tag => tag.Name);
            actualAncestorNames.Should().BeEquivalentTo("Root");
        }

        /*       Root
         *       ^  ^
         *      /  Middle
         *     /    ^
         * Left -> Right
         */

        private void Act()
        {
            var root = _sut.Create(new Tag {Name = "Root"});
            var middle = _sut.Create(new Tag {Name = "Middle"});
            var left = _sut.Create(new Tag {Name = "Left"});
            var right = _sut.Create(new Tag {Name = "Right"});
            
            middle.AddParent(root);
            right.AddParent(middle);
            left.AddParent(right);
            left.AddParent(root);

            _sut.SaveChanges();
        }
    }
}
