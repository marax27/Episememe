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
            var givenSampleUserId = "sampleUserId";

            Action act = () => MarkFavoriteMediaCommand.Create(givenMediaInstanceId, givenSampleUserId);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GivenUserIdIsNull_ArgumentNullExceptionIsThrown()
        {
            string givenUserId = null;
            var givenSampleMediaInstanceId = "abcdefgh";

            Action act = () => MarkFavoriteMediaCommand.Create(givenSampleMediaInstanceId, givenUserId);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
