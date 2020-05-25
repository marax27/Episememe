using Episememe.Application.DataTransfer;
using Episememe.Application.Features.GetFavoriteMedia;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using FluentAssertions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.GetFavoriteMedia
{
    public class WhenGettingFavoriteMedia : IDisposable
    {
        private readonly IWritableApplicationContext _contextMock;
        private readonly DbConnection _connection;

        public WhenGettingFavoriteMedia()
        {
            (_contextMock, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
        }

        [Fact]
        public void GivenUsersFavoriteMedia_CorrespondingMediaInstanceDtoAreReturned()
        {
            var givenMediaInstanceId1 = "1";
            var givenMediaInstanceId2 = "2";
            var givenUserId1 = "user1";
            var givenDataType = "png";
            AddMediaInstance(givenMediaInstanceId1, givenDataType);
            AddMediaInstance(givenMediaInstanceId2, givenDataType);
            AddFavoriteMedia(givenMediaInstanceId1, givenUserId1);
            AddFavoriteMedia(givenMediaInstanceId2, givenUserId1);

            var expectedMediaInstanceDto = new List<MediaInstanceDto>()
            {
                new MediaInstanceDto(givenMediaInstanceId1, givenDataType, new List<string>()),
                new MediaInstanceDto(givenMediaInstanceId2, givenDataType, new List<string>())
            };

            var query = GetFavoriteMediaQuery.Create(givenUserId1);
            IRequestHandler<GetFavoriteMediaQuery, IEnumerable<MediaInstanceDto>> handler =
                new GetFavoriteMediaQueryHandler(_contextMock);
            var result = handler.Handle(query, CancellationToken.None).Result;

            result.Should().BeEquivalentTo(expectedMediaInstanceDto);
        }

        private void AddMediaInstance(string id, string dataType)
        {
            var newMediaInstance = new MediaInstance()
            {
                Id = id,
                DataType = dataType
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
