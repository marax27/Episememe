using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Episememe.Application.Features.FileUpload
{
    public class FileUploadCommandHandler : IRequestHandler<FileUploadCommand>
    {
        private readonly IFileStorage _fileStorage;
        private readonly IWritableApplicationContext _context;
        private readonly ITimeProvider _timeProvider;
        private readonly IMediaIdProvider _mediaIdProvider;

        public FileUploadCommandHandler(IFileStorage fileStorage, IWritableApplicationContext context, 
            ITimeProvider timeProvider, IMediaIdProvider mediaIdProvider)
        {
            _fileStorage = fileStorage;
            _context = context;
            _timeProvider = timeProvider;
            _mediaIdProvider = mediaIdProvider;
        }

        public async Task<Unit> Handle(FileUploadCommand request, CancellationToken cancellationToken)
        {
            var instanceId = GenerateMediaInstanceId();
            var extension = Path.GetExtension(request.FormFile.FileName);
            var dataType = extension.Length > 0 ? extension.Substring(1) : string.Empty;
            await CreateMediaFile(instanceId, dataType, request.Tags, request.AuthorId, request.FormFile);

            return Unit.Value;
        }

        private async Task CreateMediaFile(string instanceId, string dataType, IEnumerable<string> tags, string? authorId,
            IFormFile formFile)
        {
            await CreateMediaFileInFileSystem(formFile, instanceId);

            using var transaction = _context.Database.BeginTransactionAsync();
            try
            {
                await CreateMediaInstanceInDatabase(instanceId, dataType, tags, authorId);
                await transaction.Result.CommitAsync(CancellationToken.None);
            }
            catch (Exception)
            {
                await transaction.Result.RollbackAsync(CancellationToken.None);
                _fileStorage.Delete(instanceId);
                throw;
            }
        }

        private async Task CreateMediaFileInFileSystem(IFormFile formFile, string filename)
        {
            await using var stream = _fileStorage.CreateNew(filename);
            await formFile.CopyToAsync(stream);
        }

        private async Task CreateMediaInstanceInDatabase(string id, string dataType, IEnumerable<string> tags, string? authorId)
        {
            var mediaInstance = new MediaInstance()
            {
                Id = id,
                DataType = dataType,
                AuthorId = authorId
            };

            ICollection<MediaTag> mediaTags = ConvertStringsToTags(tags)
                .Select(t => new MediaTag()
                {
                    MediaInstance = mediaInstance,
                    Tag = t
                })
                .ToList();

            mediaInstance.MediaTags = mediaTags;
            mediaInstance.Timestamp = _timeProvider.GetUtc();

            await _context.MediaInstances.AddAsync(mediaInstance);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        private string GenerateMediaInstanceId()
        {
            var idExists = true;
            var newId = string.Empty;
            var existingIds = _context.MediaInstances.Select(mi => mi.Id);

            while (idExists)
            {
                newId = _mediaIdProvider.Generate();

                if (!existingIds.Contains(newId))
                {
                    idExists = false;
                }
            }

            return newId;
        }

        private IEnumerable<Tag> ConvertStringsToTags(IEnumerable<string> stringTags)
        {
            var tagNames = _context.Tags.Select(t => t.Name);
            var tags = stringTags.Select(st =>
                tagNames.Contains(st) ? _context.Tags.Single(t => t.Name == st) : new Tag() { Name = st });

            return tags;
        }
    }
}
