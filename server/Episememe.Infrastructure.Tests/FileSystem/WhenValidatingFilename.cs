using System;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Episememe.Infrastructure.FileSystem;
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
            var rootDirectory = "";
            var fileSystem = new MockFileSystem();
            var sut = new FileStorage(rootDirectory, fileSystem);
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
            var rootDirectory = "";
            var fileSystem = new MockFileSystem();
            var sut = new FileStorage(rootDirectory, fileSystem);

            Action act = () => sut.CreateNew(givenFilename).Close();

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("x")]
        [InlineData("xy")]
        public void GivenShortFilename_ArgumentExceptionIsThrown(string givenFilename)
        {
            var rootDirectory = "";
            var fileSystem = new MockFileSystem();
            var sut = new FileStorage(rootDirectory, fileSystem);

            Action act = () => sut.CreateNew(givenFilename).Close();

            act.Should().Throw<ArgumentException>();
        }
    }
}
