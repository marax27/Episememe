using Episememe.Application.Exceptions;
using Episememe.Application.Features.UpdateTags;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using Episememe.Infrastructure.Database;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.FileRevision
{
    public class WhenRevisiogMedia : IDisposable
    {
        private readonly IWritableApplicationContext _contextMock;
        private readonly DbConnection _connection;

        public WhenRevisiogMedia()
        {
            (_contextMock, _connection) = GetInMemoryDatabaseContext();
        }

        [Fact]
        public void SwapTagsInInstance()
        {
            var Tags = new List<string> 
            {"sword", "shield", "minimini"};

            var command = UpdateTagsCommand.Create("k8wetest", Tags);
            var handler = new UpdateTagsCommandHandler(_contextMock);
            handler.Handle(command, CancellationToken.None).Wait();

            _contextMock.MediaInstances.Single().MediaTags.Select(t => t.Tag.Name).Should().BeEquivalentTo(Tags);
        }

        [Fact]
        public void TryEditNonexistentFile()
        {
            var Tags = new List<string> 
            {"sword", "shield", "minimini"};

            var command = UpdateTagsCommand.Create("xdxdxdxd", Tags);
            var handler = new UpdateTagsCommandHandler(_contextMock);
            
            Action act = () => handler.Handle(command, CancellationToken.None).Wait();

            act.Should().Throw<FileDoesNotExistException>();
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

            context.MediaInstances.Add(CreateExampleDatabaseInstance());
            context.SaveChanges();

            return (context, connection);
        }

        public MediaInstance CreateExampleDatabaseInstance()
        {
            var mediaInstance = new MediaInstance()
            {
                Id = "k8wetest",
                DataType = "file",
            };
            var tags = new List<string>()
            {
                "pigeons", "flying rats", "little mermaid"
            };
            ICollection<MediaTag> mediaTags = tags.Select(t => new MediaTag() 
            {
                    MediaInstance = mediaInstance,
                    Tag = new Tag() {Name = t}
            }).ToList();

            return mediaInstance;
        }

          public void Dispose()
        {
            _connection?.Dispose();
        }
    }

}