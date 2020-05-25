using System;
using Episememe.Application.Features.MarkFavoriteMedia;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.MarkFavoriteMedia
{
    public class WhenMarkingMediaAsFavorite : IDisposable
    {
        private readonly IWritableApplicationContext _contextMock;
        private readonly DbConnection _connection;

        public WhenMarkingMediaAsFavorite()
        {
            (_contextMock, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
        }

        [Fact]
        public void GivenSampleMediaAndUser_FavoriteMediaIsAdded()
        {
            var givenSampleUserId = "user1";
            var givenSampleMediaInstanceId = "1";
            AddMediaInstance(givenSampleMediaInstanceId);

            var command = MarkFavoriteMediaCommand.Create(givenSampleMediaInstanceId, givenSampleUserId);
            var handler = new MarkFavoriteMediaCommandHandler(_contextMock);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.FavoriteMedia.Should()
                .Contain(fm => fm.UserId == givenSampleUserId && fm.MediaInstanceId == givenSampleMediaInstanceId);
        }

        private void AddMediaInstance(string id)
        {
            var newMediaInstance = new MediaInstance()
            {
                Id = id,
                DataType = "png",
                IsPrivate = false
            };

            _contextMock.MediaInstances.Add(newMediaInstance);
            _contextMock.SaveChanges();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
