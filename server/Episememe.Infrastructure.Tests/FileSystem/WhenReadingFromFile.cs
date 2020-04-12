using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Episememe.Application.Exceptions;
using Episememe.Infrastructure.FileSystem;
using Episememe.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace Episememe.Infrastructure.Tests.FileSystem
{
    public class WhenReadingFromFile
    {
        [Fact]
        public void GivenTheFileExists_FileContentIsRead()
        {
            var givenOptions = OptionsFactory.Create("");
            var givenFileContent = "SampleContent";
            var fileSystem = new MockFileSystem()
                .WithFile("q/w/erty", givenFileContent);
            var sut = new FileStorage(givenOptions, fileSystem);

            var stream = sut.Read("qwerty");
            var actualFileContent = new StreamReader(stream).ReadToEnd();

            actualFileContent.Should().Be(givenFileContent);
        }

        [Fact]
        public void GivenTheFileDoesNotExist_FileDoesNotExistExceptionIsThrown()
        {
            var givenOptions = OptionsFactory.Create("");
            var fileSystem = new MockFileSystem();
            var sut = new FileStorage(givenOptions, fileSystem);

            Action act = () => sut.Read("qwerty").Close();

            act.Should().Throw<FileDoesNotExistException>();
        }
    }
}
