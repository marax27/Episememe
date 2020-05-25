using System;
using System.Collections.Generic;
using System.Threading;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Application.DataTransfer;
using Episememe.Application.Features.GetStatistic;
using MediatR;
using Moq;
using Xunit;
using FluentAssertions;

namespace Episememe.Application.Tests.FeatureTests.GetStatistics
{
    public class WhenGettingStatistics
    {
        private readonly DateTime _givenDate = new DateTime(2020, 5, 25);
        private readonly Mock<ITimeProvider> _timeProviderMock;

        public WhenGettingStatistics()
        {
            _timeProviderMock = new Mock<ITimeProvider>();
            _timeProviderMock.Setup(provider => provider.GetUtc())
                .Returns(_givenDate);
        }

        [Fact]
        public void WhenRequestingStatistics_ThenStatisticsAreReturned()
        {
            var date1 = new DateTime(2020, 5, 23, 23, 58, 1, DateTimeKind.Utc);
            var date2 = new DateTime(2020, 5, 24, 11, 03, 1, DateTimeKind.Utc);
            var date3 = new DateTime(2020, 5, 25, 0, 0, 0, DateTimeKind.Utc);
            var givenMedia = new[] 
            {
                new MediaInstance {Id = "media1", Timestamp = date1},
                new MediaInstance {Id = "media2", Timestamp = date1},
                new MediaInstance {Id = "media3", Timestamp = date2},
                new MediaInstance {Id = "media4", Timestamp = date2},
                new MediaInstance {Id = "media5", Timestamp = date2},};
            
            var expectedResult = new GetStatisticsDto {Data = new List<List<long>> 
            {
                new List<long> { (long) (date1.Date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds, 2},
                new List<long> { (long) (date2.Date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds, 5},
                new List<long> { (long) (date3.Date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds, 5}
            }};

            IApplicationContext context = CreateMockApplicationContext(givenMedia);
            var query = GetStatisticsQuery.Create();
            IRequestHandler<GetStatisticsQuery, GetStatisticsDto> sut = new GetStatisticsQueryHandler(context, _timeProviderMock.Object);
            var actualResult = sut.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void WhenRequestingStatisticsFromEmptyList_ThenListWithoutInstancesIsReturned()
        {
            var date3 = new DateTime(2020, 5, 25, 15, 34, 1, DateTimeKind.Utc);
            IEnumerable<MediaInstance> givenMedia = new List<MediaInstance>();

            var expectedResult = new GetStatisticsDto {Data = new List<List<long>> 
            {
                new List<long> { (long) (date3.Date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds, 0}
            }};

            IApplicationContext context = CreateMockApplicationContext(givenMedia);
            var query = GetStatisticsQuery.Create();
            IRequestHandler<GetStatisticsQuery, GetStatisticsDto> sut = new GetStatisticsQueryHandler(context, _timeProviderMock.Object);
            var actualResult = sut.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private IApplicationContext CreateMockApplicationContext(IEnumerable<MediaInstance> givenMedia)
        {
            var tagMock = DbSetMockFactory.Create(givenMedia);

            var mock = new Mock<IApplicationContext>();
            mock.Setup(m => m.MediaInstances).Returns(tagMock.Object);

            return mock.Object;
        }
    }
}