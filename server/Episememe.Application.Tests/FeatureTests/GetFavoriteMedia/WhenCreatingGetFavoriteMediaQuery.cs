using Episememe.Application.Features.GetFavoriteMedia;
using FluentAssertions;
using System;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.GetFavoriteMedia
{
    public class WhenCreatingGetFavoriteMediaQuery
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenUserIdIsNullOrEmpty_ArgumentNullExceptionIsThrown(string givenUserId)
        {
            Action act = () => GetFavoriteMediaQuery.Create(givenUserId);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
