using Episememe.Application.Features.MediaRevisionHistory;
using FluentAssertions;
using System;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.MediaRevisionHistory
{
    public class WhenCreatingMediaRevisionHistoryQuery
    {
        [Fact]
        public void GivenNullMediaInstanceId_ArgumentNullExceptionIsThrown()
        {
            string givenMediaInstanceId = null;

            Action act = () => MediaRevisionHistoryQuery.Create(givenMediaInstanceId);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
