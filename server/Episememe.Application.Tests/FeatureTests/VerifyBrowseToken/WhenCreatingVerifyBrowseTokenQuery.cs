using System;
using Episememe.Application.Features.VerifyBrowseToken;
using FluentAssertions;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.VerifyBrowseToken
{
    public class WhenCreatingVerifyBrowseTokenQuery
    {
        [Fact]
        public void GivenSampleTokenValue_ThenQueryContainsGivenValue()
        {
            var query = VerifyBrowseTokenQuery.Create("sampleToken");
            query.TokenValue.Should().Be("sampleToken");
        }

        [Fact]
        public void GivenEmptyString_ThenThrowsArgumentException()
        {
            Action act = () => VerifyBrowseTokenQuery.Create("");

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenNullString_ThenThrowsArgumentException()
        {
            Action act = () => VerifyBrowseTokenQuery.Create(null);

            act.Should().Throw<ArgumentException>();
        }
    }
}
