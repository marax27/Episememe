using System;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Episememe.Infrastructure.FileSystem;
using Episememe.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace Episememe.Infrastructure.Tests.FileSystem
{
    public class WhenValidatingFilename
    {
        [Theory]
        [InlineData("example")]
        [InlineData("00000")]
        [InlineData("QWERTY123qwerty")]
        [InlineData("CamelCase")]
        public void GivenLongAlphanumericFilename_FileIsCreated(string givenFilename)
        {
            var givenOptions = OptionsFactory.Create("");
            var fileSystem = new MockFileSystem();
            var sut = new FileStorage(givenOptions, fileSystem);
            var initialFileCount = fileSystem.AllFiles.Count();

            sut.CreateNew(givenFilename)
                .Close();

            fileSystem.AllFiles.Count().Should().Be(1 + initialFileCount);
        }

        [Theory]
        [InlineData("invalid-name")]
        [InlineData("lets/../escape")]
        [InlineData("0000.txt")]
        public void GivenFilenameWithNonAlphanumericCharacters_ArgumentExceptionIsThrown(string givenFilename)
        {
            var givenOptions = OptionsFactory.Create("");
            var fileSystem = new MockFileSystem();
            var sut = new FileStorage(givenOptions, fileSystem);

            Action act = () => sut.CreateNew(givenFilename).Close();

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("x")]
        public void GivenShortFilename_ArgumentExceptionIsThrown(string givenFilename)
        {
            var givenOptions = OptionsFactory.Create("");
            var fileSystem = new MockFileSystem();
            var sut = new FileStorage(givenOptions, fileSystem);

            Action act = () => sut.CreateNew(givenFilename).Close();

            act.Should().Throw<ArgumentException>();
        }
    }
}
