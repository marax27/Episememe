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
    public class WhenCreatingNewFile
    {
        [Fact]
        public void GivenEmptyFileSystem_ThenFileIsCreated()
        {
            var givenOptions = OptionsFactory.Create("");
            var fileSystem = new MockFileSystem();
            var sut = new FileStorage(givenOptions, fileSystem);

            sut.CreateNew("abcdef")
                .Close();

            fileSystem.GetFile("a/b/cdef").Should().NotBeNull();
        }

        [Fact]
        public void GivenFileSystemContainingFileWithDifferentName_ThenFileIsCreated()
        {
            var givenOptions = OptionsFactory.Create("");
            var fileSystem = new MockFileSystem()
                .WithFile("OtherFile", "file content");
            var sut = new FileStorage(givenOptions, fileSystem);

            sut.CreateNew("abcdef")
                .Close();

            fileSystem.GetFile("a/b/cdef").Should().NotBeNull();
        }

        [Fact]
        public void GivenFileSystemContainingFileWithTheSameName_ThenFileExistsExceptionIsThrown()
        {
            var givenOptions = OptionsFactory.Create("");
            var fileSystem = new MockFileSystem()
                .WithFile("a/b/cdef", "");
            var sut = new FileStorage(givenOptions, fileSystem);

            Action act = () => sut.CreateNew("abcdef").Close();

            act.Should().Throw<FileExistsException>();
        }

        [Fact]
        public void GivenEmptyFileSystemAndNonEmptyRootDirectory_ThenFileIsCreatedInsideRootDirectory()
        {
            var rootDirectory = "root-dir";
            var givenOptions = OptionsFactory.Create(rootDirectory);
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(rootDirectory);
            var sut = new FileStorage(givenOptions, fileSystem);

            sut.CreateNew("abcdef")
                .Close();

            var expectedPath = Path.Combine(rootDirectory, "a", "b", "cdef");
            fileSystem.GetFile(expectedPath).Should().NotBeNull();
        }
    }
}
