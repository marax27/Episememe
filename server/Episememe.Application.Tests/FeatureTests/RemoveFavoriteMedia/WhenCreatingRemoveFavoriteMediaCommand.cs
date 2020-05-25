using Episememe.Application.Features.RemoveFavoriteMedia;
using FluentAssertions;
using System;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.RemoveFavoriteMedia
{
    public class WhenCreatingRemoveFavoriteMediaCommand
    {
        [Fact]
        public void GivenMediaInstanceIdIsNull_ArgumentNullExceptionIsThrown()
        {
            string givenMediaInstanceId = null;
            var givenSampleUser = "sampleUser";

            Action act = () => RemoveFavoriteMediaCommand.Create(givenMediaInstanceId, givenSampleUser);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenUserIdIsNullOrEmpty_ArgumentNullExceptionIsThrown(string givenUserId)
        {
            var givenSampleMediaInstanceId = "abcdefgh";

            Action act = () => RemoveFavoriteMediaCommand.Create(givenSampleMediaInstanceId, givenUserId);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
