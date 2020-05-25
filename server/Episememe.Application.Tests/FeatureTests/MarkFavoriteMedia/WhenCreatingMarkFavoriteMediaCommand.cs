using Episememe.Application.Features.MarkFavoriteMedia;
using FluentAssertions;
using System;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.MarkFavoriteMedia
{
    public class WhenCreatingMarkFavoriteMediaCommand
    {
        [Fact]
        public void GivenMediaInstanceIdIsNull_ArgumentNullExceptionIsThrown()
        {
            string givenMediaInstanceId = null;
            var givenSampleUser = "sampleUser";

            Action act = () => MarkFavoriteMediaCommand.Create(givenMediaInstanceId, givenSampleUser);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenUserIdIsNullOrEmpty_ArgumentNullExceptionIsThrown(string givenUserId)
        {
            var givenSampleMediaInstanceId = "abcdefgh";

            Action act = () => MarkFavoriteMediaCommand.Create(givenSampleMediaInstanceId, givenUserId);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
