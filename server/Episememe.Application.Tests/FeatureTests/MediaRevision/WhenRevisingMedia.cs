using Episememe.Application.Exceptions;
using Episememe.Application.Features.UpdateTags;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using Moq;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.MediaRevision
{
    public class WhenRevisingMedia : IDisposable
    {
        private readonly DateTime _givenProvidedUtcDateTime = new DateTime(2010, 1, 1, 1, 0, 0);

        private readonly IWritableApplicationContext _contextMock;
        private readonly Mock<ITimeProvider> _timeProviderMock;

        private readonly DbConnection _connection;

        public WhenRevisingMedia()
        {
            (_contextMock, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();

            _timeProviderMock = new Mock<ITimeProvider>();
            _timeProviderMock.Setup(provider => provider.GetUtc())
                .Returns(_givenProvidedUtcDateTime);
        }

        [Fact]
        public void GivenMediaTagsAreUpdated_ThenExpectedMediaTagsAreInDatabase()
        {
            _contextMock.MediaInstances.Add(CreateExampleDatabaseInstance());
            _contextMock.SaveChanges();
            var tags = new List<string>
            {
                "sword", "shield", "minimini"
            };

            var command = UpdateTagsCommand.Create("k8wetest", tags, string.Empty);
            var handler = new UpdateTagsCommandHandler(_contextMock, _timeProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaInstances.Single().MediaTags.Select(t => t.Tag.Name).Should().BeEquivalentTo(tags);
        }

        [Fact]
        public void GivenNonexistentFileID_ThenExceptionIsThrown()
        {
            _contextMock.MediaInstances.Add(CreateExampleDatabaseInstance());
            _contextMock.SaveChanges();
            var tags = new List<string>
            {
                "sword", "shield", "minimini"
            };

            var command = UpdateTagsCommand.Create("xdxdxdxd", tags, string.Empty);
            var handler = new UpdateTagsCommandHandler(_contextMock, _timeProviderMock.Object);

            Action act = () => handler.Handle(command, CancellationToken.None).Wait();

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void GivenEmptyTagsList_ThenArgumentNullExceptionThrown()
        {
            _contextMock.MediaInstances.Add(CreateExampleDatabaseInstance());
            _contextMock.SaveChanges();
            var tags = new List<string>();
            Action act = () => UpdateTagsCommand.Create("k8wetest", tags, string.Empty);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GivenAnotherUsersPrivateMedia_ThenMediaDoesNotBelongToUserIsThrown()
        {
            var givenUser1 = "user1";
            var givenUser2 = "user2";
            var givenMediaId = "abcdefgh";
            var givenMediaInstance = new MediaInstance()
            {
                Id = givenMediaId,
                AuthorId = givenUser1,
                DataType = "png",
                IsPrivate = true
            };
            var givenTags = new List<string>()
            {
                "test everything"
            };
            _contextMock.MediaInstances.Add(givenMediaInstance);
            _contextMock.SaveChanges();

            var command = UpdateTagsCommand.Create(givenMediaId, givenTags, givenUser2);
            var handler = new UpdateTagsCommandHandler(_contextMock, _timeProviderMock.Object);
            Action act = () => handler.Handle(command, CancellationToken.None).Wait();

            act.Should().Throw<MediaDoesNotBelongToUserException>();
        }

        [Fact]
        public void GivenMediaTagsAreUpdated_MediaChangeIsSavedInDatabase()
        {
            var givenUser = "user";
            var givenMediaId = "abcdefgh";
            var givenMediaInstance = new MediaInstance
            {
                Id = givenMediaId,
                AuthorId = givenUser,
                DataType = "png"
            };
            var givenTags = new List<string>
            {
                "test everything"
            };
            _contextMock.MediaInstances.Add(givenMediaInstance);
            _contextMock.SaveChanges();

            var command = UpdateTagsCommand.Create(givenMediaId, givenTags, givenUser);
            var handler = new UpdateTagsCommandHandler(_contextMock, _timeProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaChanges.Should().ContainSingle();
        }

        [Fact]
        public void GivenMediaChangeIsSavedInDatabase_MediaChangeIsOfTypeUpdate()
        {
            var givenUser = "user";
            var givenMediaId = "abcdefgh";
            var givenMediaInstance = new MediaInstance
            {
                Id = givenMediaId,
                AuthorId = givenUser,
                DataType = "png"
            };
            var givenTags = new List<string>
            {
                "test everything"
            };
            _contextMock.MediaInstances.Add(givenMediaInstance);
            _contextMock.SaveChanges();

            var command = UpdateTagsCommand.Create(givenMediaId, givenTags, givenUser);
            var handler = new UpdateTagsCommandHandler(_contextMock, _timeProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaChanges.Should().ContainSingle(mc => mc.Type == MediaChangeType.Update);
        }

        public MediaInstance CreateExampleDatabaseInstance()
        {
            var mediaInstance = new MediaInstance()
            {
                Id = "k8wetest",
                DataType = "file",
            };
            var tags = new List<string>()
            {
                "pigeons", "flying rats", "little mermaid"
            };
            ICollection<MediaTag> mediaTags = tags.Select(t => new MediaTag()
            {
                MediaInstance = mediaInstance,
                Tag = new Tag() { Name = t }
            }).ToList();

            return mediaInstance;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }

}