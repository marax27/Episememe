using System;
using System.IO.Abstractions.TestingHelpers;
using Episememe.Application.Exceptions;
using Episememe.Infrastructure.FileSystem;
using Episememe.Infrastructure.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace Episememe.Infrastructure.Tests.FileSystem
{
    public class WhenDeletingFile
    {
        [Fact]
        public void GivenTheFileExists_ThenFileIsDeleted()
        {
            var givenOptions = OptionsFactory.Create("");
            var fileSystem = new MockFileSystem()
                .WithFile("q/w/qwerty", "Sample content");
            var sut = new FileStorage(givenOptions, fileSystem);

            sut.Delete("qwerty");

            fileSystem.GetFile("q/w/qwerty").Should().BeNull();
        }

        [Fact]
        public void GivenTheFileDoesNotExist_FileDoesNotExistExceptionIsThrown()
        {
            var givenOptions = OptionsFactory.Create("");
            var fileSystem = new MockFileSystem();
            var sut = new FileStorage(givenOptions, fileSystem);

            Action act = () => sut.Delete("qwerty");

            act.Should().Throw<FileDoesNotExistException>();
        }
    }
}
