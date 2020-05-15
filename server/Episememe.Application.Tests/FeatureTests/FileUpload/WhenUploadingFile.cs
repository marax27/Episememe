using Episememe.Application.Features.FileUpload;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using Episememe.Infrastructure.Database;
using Episememe.Infrastructure.FileSystem;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
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
            (_contextMock, _connection) = GetInMemoryDatabaseContext();

            _timeProviderMock = new Mock<ITimeProvider>();
            _timeProviderMock.Setup(provider => provider.GetUtc())
                .Returns(_givenDate);

            _mediaIdProviderMock = new Mock<IMediaIdProvider>();
            _mediaIdProviderMock.SetupSequence(provider => provider.Generate())
                .Returns(_givenMediaInstanceId)
                .Returns(_givenAlternativeMediaInstanceId);
        }

        [Fact]
        public void GivenNullFormFile_ExceptionIsThrown()
        {
            var givenTags = new List<string>()
            {
                "pigeons", "flying rats"
            };

            Assert.Throws<ArgumentNullException>(() => FileUploadCommand.Create(null, givenTags, string.Empty));
        }

        [Fact]
        public void GivenNullTags_ExceptionIsThrown()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = CreateTestFormFile(givenFileName, givenFileContent);

            Assert.Throws<ArgumentNullException>(() => FileUploadCommand.Create(givenFormFile, null, string.Empty));
        }

        [Fact]
        public void GivenNewMediaIsUploadedSuccessfully_MediaIsCreatedInDatabase()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = CreateTestFormFile(givenFileName, givenFileContent);
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
            var givenFormFile = CreateTestFormFile(givenFileName, givenFileContent);
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
            var givenFormFile = CreateTestFormFile(givenFileName, givenFileContent);
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
            var givenFormFile = CreateTestFormFile(givenFileName, givenFileContent);
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
            var givenFormFile = CreateTestFormFile(givenFileName, givenFileContent);
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
            var givenFormFile = CreateTestFormFile(givenFileName, givenFileContent);
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
            var givenFormFile = CreateTestFormFile(givenFileName + "." + givenExtension, givenFileContent);
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
            var formFile = CreateTestFormFile(givenFileName, givenFileContent);
            var givenTag = "pigeons";
            var givenTags = new List<string>()
            {
                givenTag
            };

            _contextMock.Tags.Should().BeEmpty();

            var command = FileUploadCommand.Create(formFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.Tags.Should().HaveCount(1);
            _contextMock.Tags.Select(t => t.Name).Single().Should().BeEquivalentTo(givenTag);
        }

        [Fact]
        public void GivenMediaWithExistingTag_ExistingTagIsUsed()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var formFile = CreateTestFormFile(givenFileName, givenFileContent);
            var givenTag = "pigeons";
            var givenTags = new List<string>()
            {
                givenTag
            };
            _contextMock.Tags.Add(new Tag() { Name = givenTag });
            _contextMock.SaveChanges();

            _contextMock.Tags.Should().HaveCount(1);

            var command = FileUploadCommand.Create(formFile, givenTags, string.Empty);
            var handler = new FileUploadCommandHandler(_fileStorageMock, _contextMock,
                _timeProviderMock.Object, _mediaIdProviderMock.Object);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.Tags.Should().HaveCount(1);
            _contextMock.Tags.Select(t => t.Name).Single().Should().BeEquivalentTo(givenTag);
        }

        private (IWritableApplicationContext, DbConnection) GetInMemoryDatabaseContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();

            return (context, connection);
        }

        private IFormFile CreateTestFormFile(string fileName, string fileContent)
        {
            var bytes = Encoding.UTF8.GetBytes(fileContent);

            return new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: fileName
            );
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
