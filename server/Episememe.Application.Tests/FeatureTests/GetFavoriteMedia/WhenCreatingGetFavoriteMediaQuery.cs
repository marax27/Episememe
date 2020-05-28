using Episememe.Application.Features.GetFavoriteMedia;
using FluentAssertions;
using System;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.GetFavoriteMedia
{
    public class WhenCreatingGetFavoriteMediaQuery
    {
        [Fact]
        public void GivenUserIdIsNull_ArgumentNullExceptionIsThrown()
        {
            string givenUserId = null;
            Action act = () => GetFavoriteMediaQuery.Create(givenUserId);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
