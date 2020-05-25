using Episememe.Application.Features.RemoveFavoriteMedia;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using FluentAssertions;
using System;
using System.Data.Common;
using System.Threading;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.RemoveFavoriteMedia
{
    public class WhenRemovingMediaFromFavorites : IDisposable
    {
        private readonly IWritableApplicationContext _contextMock;
        private readonly DbConnection _connection;

        public WhenRemovingMediaFromFavorites()
        {
            (_contextMock, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
        }

        [Fact]
        public void GivenDatabaseWithSingleFavoriteMedia_ThenFavoriteMediaTableIsEmpty()
        {
            var givenSampleUserId = "user1";
            var givenSampleMediaInstanceId = "1";
            AddMediaInstance(givenSampleMediaInstanceId);
            AddFavoriteMedia(givenSampleMediaInstanceId, givenSampleUserId);

            var command = RemoveFavoriteMediaCommand.Create(givenSampleMediaInstanceId, givenSampleUserId);
            var handler = new RemoveFavoriteMediaCommandHandler(_contextMock);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.FavoriteMedia.Should().BeEmpty();
        }

        [Fact]
        public void GivenNoMatchingFavoriteMediaInDatabase_ExceptionIsThrown()
        {
            var givenUserId = "user1";
            var givenMediaInstanceId = "1";
            var givenDifferentMediaInstanceId = "2";
            AddMediaInstance(givenDifferentMediaInstanceId);
            AddFavoriteMedia(givenDifferentMediaInstanceId, givenUserId);

            var command = RemoveFavoriteMediaCommand.Create(givenMediaInstanceId, givenUserId);
            var handler = new RemoveFavoriteMediaCommandHandler(_contextMock);
            Action act = () => handler.Handle(command, CancellationToken.None).Wait();

            act.Should().Throw<Exception>();
        }

        private void AddMediaInstance(string id)
        {
            var newMediaInstance = new MediaInstance()
            {
                Id = id,
                DataType = "png"
            };

            _contextMock.MediaInstances.Add(newMediaInstance);
            _contextMock.SaveChanges();
        }

        private void AddFavoriteMedia(string mediaInstanceId, string userId)
        {
            var newFavoriteMedia = new FavoriteMedia()
            {
                MediaInstanceId = mediaInstanceId,
                UserId = userId
            };

            _contextMock.FavoriteMedia.Add(newFavoriteMedia);
            _contextMock.SaveChanges();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
