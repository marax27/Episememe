using Episememe.Application.Features.FileUpload;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Infrastructure.FileSystem;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.FileUpload
{
    public class WhenUploadingFile : IDisposable
    {
        private readonly string _givenMediaInstanceId = "abcdefgh";
        private readonly string _givenAlternativeMediaInstanceId = "qwertyui";
        private readonly DateTime _givenDate = new DateTime(2010, 1, 1);

        private readonly IFileStorage _fileStorageMock;
        private readonly IWritableApplicationContext _contextMock;
        private readonly Mock<ITimeProvider> _timeProviderMock;
        private readonly Mock<IMediaIdProvider> _mediaIdProviderMock;

        private readonly DbConnection _connection;

        public WhenUploadingFile()
        {
            var fileSystemMock = new MockFileSystem();
            var fileStorageSettings = Options.Create(new FileStorageSettings()
            {
                RootDirectory = ""
            });
            _fileStorageMock = new FileStorage(fileStorageSettings, fileSystemMock);
            (_contextMock, _connection) = InMemoryDatabaseFactory.CreateSqliteDbContext();

            _timeProviderMock = new Mock<ITimeProvider>();
            _timeProviderMock.Setup(provider => provider.GetUtc())
                .Returns(_givenDate);

            _mediaIdProviderMock = new Mock<IMediaIdProvider>();
            _mediaIdProviderMock.SetupSequence(provider => provider.Generate())
                .Returns(_givenMediaInstanceId)
                .Returns(_givenAlternativeMediaInstanceId);
        }

        [Fact]
        public void GivenNewMediaIsUploadedSuccessfully_MediaIsCreatedInDatabase()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new List<string>()
            {
                "pigeons", "flying rats"
            };

            var command = FileUploadCommand.Create(givenFormFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaInstances.Find(_givenMediaInstanceId).Should().NotBeNull();
        }

        [Fact]
        public void GivenNewMediaIsUploadedSuccessfully_MediaIsCreatedInFileSystem()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new List<string>()
            {
                "pigeons", "flying rats"
            };

            var command = FileUploadCommand.Create(givenFormFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            var streamContent = GetFileContent(_fileStorageMock.Read(_givenMediaInstanceId));
            streamContent.Should().BeEquivalentTo(givenFileContent);
        }

        [Fact]
        public void GivenMediaWithEmptyTagCollection_MediaIsCreatedWithoutTags()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new List<string>();

            var command = FileUploadCommand.Create(givenFormFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaInstances.Find(_givenMediaInstanceId).MediaTags.Should().BeEmpty();
        }

        [Fact]
        public void GivenThatFileWithGivenIdAlreadyExists_MediaIsCreatedWithDifferentId()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new List<string>()
            {
                "pigeons", "flying rats"
            };

            _fileStorageMock.CreateNew(_givenMediaInstanceId);

            var command = FileUploadCommand.Create(givenFormFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            var streamContent = GetFileContent(_fileStorageMock.Read(_givenAlternativeMediaInstanceId));
            streamContent.Should().BeEquivalentTo(givenFileContent);
        }

        [Fact]
        public void GivenMediaWithNoFileExtension_DataTypeIsEmptyString()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new List<string>()
            {
                "pigeons", "flying rats"
            };

            var command = FileUploadCommand.Create(givenFormFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaInstances.Find(_givenMediaInstanceId).DataType.Should().BeEmpty();
        }

        [Fact]
        public void GivenMediaWithSingleDotAtFileNameEnd_DataTypeIsEmptyString()
        {
            var givenFileName = "newFile.";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new List<string>()
            {
                "pigeons", "flying rats"
            };

            var command = FileUploadCommand.Create(givenFormFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaInstances.Find(_givenMediaInstanceId).DataType.Should().BeEmpty();
        }

        [Fact]
        public void GivenMediaWithValidExtension_DataTypeIsEqualToFileExtension()
        {
            var givenFileName = "newFile";
            var givenExtension = "png";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName + "." + givenExtension, givenFileContent);
            var givenTags = new List<string>()
            {
                "pigeons", "flying rats"
            };

            var command = FileUploadCommand.Create(givenFormFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaInstances.Find(_givenMediaInstanceId).DataType.Should().BeEquivalentTo(givenExtension);
        }

        [Fact]
        public void GivenMediaWithNewTag_NewTagIsCreatedInDatabase()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var formFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTag = "pigeons";
            var givenTags = new List<string>()
            {
                givenTag
            };

            var command = FileUploadCommand.Create(formFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.Tags.Should().ContainSingle(t => t.Name == givenTag);
        }

        [Fact]
        public void GivenMediaWithExistingTag_ExistingTagIsUsed()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var formFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTag = "pigeons";
            var givenTags = new List<string>()
            {
                givenTag
            };
            _contextMock.Tags.Add(new Tag() { Name = givenTag });
            _contextMock.SaveChanges();

            var command = FileUploadCommand.Create(formFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.Tags.Should().ContainSingle(t => t.Name == givenTag);
        }

        private string GetFileContent(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            var content = streamReader.ReadToEnd();
            return content;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
