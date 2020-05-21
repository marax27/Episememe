using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Episememe.Application.Features.IssueBrowseToken;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.IssueBrowseToken
{
    public class WhenIssuingBrowseToken
    {
        private const string SampleUserId = "user";
        private readonly DateTime _givenDate = new DateTime(2020, 5, 1);
        private readonly ISet<BrowseToken> _givenBrowseTokens = new HashSet<BrowseToken>
        {
            new BrowseToken {Id = "xyz", ExpirationTime = new DateTime(2020, 3, 1)},
            new BrowseToken {Id = "qwe", ExpirationTime = new DateTime(2020, 6, 1)}
        };

        private readonly Mock<ITimeProvider> _timeProviderMock;
        private readonly Mock<DbSet<BrowseToken>> _browseTokensMock;
        private readonly Mock<IAuthorizationContext> _authorizationContextMock;

        private readonly ISet<BrowseToken> _addedTokens;
        private readonly ISet<BrowseToken> _removedTokens;

        public WhenIssuingBrowseToken()
        {
            _addedTokens = new HashSet<BrowseToken>();
            _removedTokens = new HashSet<BrowseToken>();

            _timeProviderMock = new Mock<ITimeProvider>();
            _timeProviderMock.Setup(provider => provider.GetUtc())
                .Returns(_givenDate);

            _browseTokensMock = DbSetMockFactory.Create(_givenBrowseTokens);
            _browseTokensMock.StoreAddedEntitiesIn(_addedTokens);
            _browseTokensMock.StoreRemovedEntitiesIn(_removedTokens);

            _authorizationContextMock = new Mock<IAuthorizationContext>();
            _authorizationContextMock.Setup(context => context.BrowseTokens)
                .Returns(_browseTokensMock.Object);
        }

        [Fact]
        public void GivenSampleToken_ThenSingleTokenIsAdded()
        {
            var command = IssueBrowseTokenCommand.Create("AB12ab", SampleUserId);

            var sut = new IssueBrowseTokenCommandHandler(_authorizationContextMock.Object, _timeProviderMock.Object);
            sut.Handle(command, CancellationToken.None).Wait();

            _addedTokens.Count.Should().Be(1);
        }

        [Fact]
        public void GivenSampleToken_ThenTokenHasExpectedValue()
        {
            var command = IssueBrowseTokenCommand.Create("AB12ab", SampleUserId);

            var sut = new IssueBrowseTokenCommandHandler(_authorizationContextMock.Object, _timeProviderMock.Object);
            sut.Handle(command, CancellationToken.None).Wait();

            _addedTokens.Should().Contain(token => token.Id == "AB12ab");
        }

        [Fact]
        public void GivenSampleToken_ThenExpirationTimeIsInTheFuture()
        {
            var command = IssueBrowseTokenCommand.Create("AB12ab", SampleUserId);

            var sut = new IssueBrowseTokenCommandHandler(_authorizationContextMock.Object, _timeProviderMock.Object);
            sut.Handle(command, CancellationToken.None).Wait();

            _addedTokens.Should().OnlyContain(token => token.ExpirationTime > _givenDate);
        }

        [Fact]
        public void GivenSampleToken_ThenExpiredTokensAreRemoved()
        {
            var expectedRemovedTokens = _givenBrowseTokens.Where(token => token.ExpirationTime <= _givenDate);
            var command = IssueBrowseTokenCommand.Create("AB12ab", SampleUserId);

            var sut = new IssueBrowseTokenCommandHandler(_authorizationContextMock.Object, _timeProviderMock.Object);
            sut.Handle(command, CancellationToken.None).Wait();

            _removedTokens.Should().BeEquivalentTo(expectedRemovedTokens);
        }
    }
}
