using Episememe.Application.DataTransfer;
using Episememe.Application.Features.MediaRevisionHistory;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using FluentAssertions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using Episememe.Application.Exceptions;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.MediaRevisionHistory
{
    public class WhenGettingMediaRevisionHistory : IDisposable
    {
        private readonly IWritableApplicationContext _contextMock;
        private readonly DbConnection _connection;

        public WhenGettingMediaRevisionHistory()
        {
            (_contextMock, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();
        }

        [Fact]
        public void GivenNoMediaChanges_EmptyCollectionIsReturned()
        {
            var givenMediaInstanceId = "abcdefgh";

            var query = MediaRevisionHistoryQuery.Create(givenMediaInstanceId, string.Empty);
            IRequestHandler<MediaRevisionHistoryQuery, IEnumerable<MediaRevisionHistoryDto>> handler =
                new MediaRevisionHistoryQueryHandler(_contextMock);
            var result = handler.Handle(query, CancellationToken.None).Result;

            result.Should().BeEmpty();
        }

        [Fact]
        public void GivenMediaInstanceWithMediaChanges_CorrespondingMediaRevisionHistoryDtoAreReturned()
        {
            var givenUserId = "user";
            var givenMediaInstanceId = "abcdefgh";
            var givenMediaCreationTime = new DateTime(2010, 1, 1, 0, 0, 0);
            var givenMediaUpdateTime = new DateTime(2010, 1, 1, 1, 0, 0);
            var givenMediaChanges = new List<MediaChange>
            {
                new MediaChange {MediaInstanceId = givenMediaInstanceId, UserId = givenUserId,
                    Timestamp = givenMediaCreationTime, Type = MediaChangeType.Create},
                new MediaChange {MediaInstanceId = givenMediaInstanceId, UserId = givenUserId,
                    Timestamp = givenMediaUpdateTime, Type = MediaChangeType.Update},
            };
            var givenMediaInstance = new MediaInstance
            {
                Id = givenMediaInstanceId,
                DataType = "png",
                MediaChanges = givenMediaChanges
            };
            AddMediaInstance(givenMediaInstance);

            var expectedMediaRevisionHistoryDtos = new List<MediaRevisionHistoryDto>
            {
                new MediaRevisionHistoryDto(givenUserId, MediaChangeType.Create, givenMediaCreationTime),
                new MediaRevisionHistoryDto(givenUserId, MediaChangeType.Update, givenMediaUpdateTime)
            };

            var query = MediaRevisionHistoryQuery.Create(givenMediaInstanceId, givenUserId);
            IRequestHandler<MediaRevisionHistoryQuery, IEnumerable<MediaRevisionHistoryDto>> handler =
                new MediaRevisionHistoryQueryHandler(_contextMock);
            var result = handler.Handle(query, CancellationToken.None).Result;

            result.Should().BeEquivalentTo(expectedMediaRevisionHistoryDtos);
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
            AddMediaInstance(givenMediaInstance);

            var query = MediaRevisionHistoryQuery.Create(givenMediaId, givenUser2);
            IRequestHandler<MediaRevisionHistoryQuery, IEnumerable<MediaRevisionHistoryDto>> handler =
                new MediaRevisionHistoryQueryHandler(_contextMock);
            Action act = () => handler.Handle(query, CancellationToken.None).Wait();

            act.Should().Throw<MediaDoesNotBelongToUserException>();
        }

        private void AddMediaInstance(MediaInstance mediaInstance)
        {
            _contextMock.MediaInstances.Add(mediaInstance);
            _contextMock.SaveChanges();
        }


        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
