using Episememe.Application.DataTransfer;
using Episememe.Application.Features.FileUpload;
using Episememe.Application.Interfaces;
using Episememe.Application.Tests.Helpers;
using Episememe.Domain.Entities;
using Episememe.Infrastructure.FileSystem;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Data.Common;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using Episememe.Domain.HelperEntities;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.FileUpload
{
    public class WhenUploadingFile : IDisposable
    {
        private readonly string _givenMediaInstanceId = "abcdefgh";
        private readonly string _givenAlternativeMediaInstanceId = "qwertyui";
        private readonly DateTime _givenDate = new DateTime(2010, 1, 1);
        private readonly DateTime _givenProvidedUtcDateTime = new DateTime(2010, 1, 1, 1, 0, 0);

        private readonly IFileStorage _fileStorageMock;
        private readonly IWritableApplicationContext _contextMock;
        private readonly Mock<IMediaIdProvider> _mediaIdProviderMock;
        private readonly Mock<ITimeProvider> _timeProviderMock;

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

            _mediaIdProviderMock = new Mock<IMediaIdProvider>();
            _mediaIdProviderMock.SetupSequence(provider => provider.Generate())
                .Returns(_givenMediaInstanceId)
                .Returns(_givenAlternativeMediaInstanceId);

            _timeProviderMock = new Mock<ITimeProvider>();
            _timeProviderMock.Setup(provider => provider.GetUtc())
                .Returns(_givenProvidedUtcDateTime);
        }

        [Fact]
        public void GivenNewMediaIsUploadedSuccessfully_MediaIsCreatedInDatabase()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new[] {"pigeons", "flying rats"};
            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);

            var command = FileUploadCommand.Create(givenFormFile, givenMediaDto, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaInstances.Find(_givenMediaInstanceId).Should().NotBeNull();
        }

        [Fact]
        public void GivenNewMediaIsUploadedSuccessfully_MediaIsCreatedInFileSystem()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new[] {"pigeons", "flying rats"};
            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);

            var command = FileUploadCommand.Create(givenFormFile, givenMediaDto, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
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
            var givenTags = new string[]{};
            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);

            var command = FileUploadCommand.Create(givenFormFile, givenMediaDto, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaInstances.Find(_givenMediaInstanceId).MediaTags.Should().BeEmpty();
        }

        [Fact]
        public void GivenThatFileWithGivenIdAlreadyExists_MediaIsCreatedWithDifferentId()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new [] {"pigeons", "flying rats"};
            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);

            _fileStorageMock.CreateNew(_givenMediaInstanceId);

            var command = FileUploadCommand.Create(givenFormFile, givenMediaDto, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
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
            var givenTags = new[] {"pigeons", "flying rats"};
            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);

            var command = FileUploadCommand.Create(givenFormFile, givenMediaDto, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaInstances.Find(_givenMediaInstanceId).DataType.Should().BeEmpty();
        }

        [Fact]
        public void GivenMediaWithSingleDotAtFileNameEnd_DataTypeIsEmptyString()
        {
            var givenFileName = "newFile.";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new[]{ "pigeons", "flying rats" };
            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);

            var command = FileUploadCommand.Create(givenFormFile, givenMediaDto, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
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
            var givenTags = new[]{ "pigeons", "flying rats" };
            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);

            var command = FileUploadCommand.Create(givenFormFile, givenMediaDto, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
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
            var givenTags = new[] { givenTag };
            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);

            var command = FileUploadCommand.Create(formFile, givenMediaDto, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
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
            var givenTags = new[] { givenTag };
            _contextMock.Tags.Add(new Tag() { Name = givenTag });
            _contextMock.SaveChanges();

            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);

            var command = FileUploadCommand.Create(formFile, givenMediaDto, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.Tags.Should().ContainSingle(t => t.Name == givenTag);
        }

        [Fact]
        public void GivenNewMediaIsUploaded_MediaChangeIsSavedInDatabase()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new[] { "pigeons", "flying rats" };
            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);
            var givenUserId = "user";

            var command = FileUploadCommand.Create(givenFormFile, givenMediaDto, givenUserId);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaChanges.Should().ContainSingle(mc => mc.MediaInstanceId == _givenMediaInstanceId
                                                             && mc.UserId == givenUserId);
        }

        [Fact]
        public void GivenMediaChangeIsSavedInDatabase_MediaChangeIsOfTypeCreate()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenTags = new[] { "pigeons", "flying rats" };
            var givenMediaDto = new FileUploadDto(givenTags, _givenDate, false);
            var givenUserId = "user";

            var command = FileUploadCommand.Create(givenFormFile, givenMediaDto, givenUserId);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _mediaIdProviderMock.Object, _timeProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaChanges.Should().ContainSingle(mc => mc.MediaInstanceId == _givenMediaInstanceId
                                                             && mc.UserId == givenUserId
                                                             && mc.Type == MediaChangeType.Create);
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
