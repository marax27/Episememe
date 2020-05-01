using System;
using System.Collections.Generic;
using System.Threading;
using Episememe.Application.Features.VerifyBrowseToken;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.VerifyBrowseToken
{
    public class WhenVerifyingBrowseToken
    {
        private readonly Mock<ITimeProvider> _timeProviderMock;
        private readonly Mock<DbSet<BrowseToken>> _browseTokensMock;
        private readonly Mock<IAuthorizationContext> _authorizationContextMock;

        public WhenVerifyingBrowseToken()
        {
            _timeProviderMock = new Mock<ITimeProvider>();
            _timeProviderMock.Setup(provider => provider.GetUtc())
                .Returns(_givenDate);

            _browseTokensMock = DbSetMockFactory.Create(_givenBrowseTokens);

            _authorizationContextMock = new Mock<IAuthorizationContext>();
            _authorizationContextMock.Setup(context => context.BrowseTokens)
                .Returns(_browseTokensMock.Object);
        }

        private readonly DateTime _givenDate = new DateTime(2020, 5, 1, 10, 0, 0);
        private readonly ISet<BrowseToken> _givenBrowseTokens = new HashSet<BrowseToken>
        {
            new BrowseToken
            {
                Id = "VeryOld",
                ExpirationTime = new DateTime(2020, 3, 1)
            },
            new BrowseToken
            {
                Id = "RecentlyExpired",
                ExpirationTime = new DateTime(2020, 5, 1, 9, 59, 59)
            },
            new BrowseToken
            {
                Id = "ExpiringInAFewSeconds",
                ExpirationTime = new DateTime(2020, 5, 1, 10, 0, 1)
            },
            new BrowseToken
            {
                Id = "Valid",
                ExpirationTime = new DateTime(2020, 6, 1)
            }
        };

        [Theory]
        [InlineData("Valid")]
        [InlineData("ExpiringInAFewSeconds")]
        public void GivenValidTokenValue_ThenReturnsTrue(string givenTokenValue)
        {
            var query = VerifyBrowseTokenQuery.Create(givenTokenValue);

            IRequestHandler<VerifyBrowseTokenQuery, bool> sut = new VerifyBrowseTokenQueryHandler(_authorizationContextMock.Object, _timeProviderMock.Object);
            var actualResult = sut.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeTrue();
        }

        [Theory]
        [InlineData("VeryOld")]
        [InlineData("RecentlyExpired")]
        public void GivenExpiredTokenValue_ThenReturnsFalse(string givenTokenValue)
        {
            var query = VerifyBrowseTokenQuery.Create(givenTokenValue);

            IRequestHandler<VerifyBrowseTokenQuery, bool> sut = new VerifyBrowseTokenQueryHandler(_authorizationContextMock.Object, _timeProviderMock.Object);
            var actualResult = sut.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeFalse();
        }

        [Fact]
        public void GivenNonexistentTokenValue_ThenReturnsFalse()
        {
            var query = VerifyBrowseTokenQuery.Create("NonexistentToken");

            IRequestHandler<VerifyBrowseTokenQuery, bool> sut = new VerifyBrowseTokenQueryHandler(_authorizationContextMock.Object, _timeProviderMock.Object);
            var actualResult = sut.Handle(query, CancellationToken.None).Result;

            actualResult.Should().BeFalse();
        }
    }
}
