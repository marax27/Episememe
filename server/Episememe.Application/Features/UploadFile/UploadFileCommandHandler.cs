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

namespace Episememe.Application.Features.UploadFile
{
    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand>
    {
        private readonly IFileStorage _fileStorage;
        private readonly IWritableApplicationContext _context;
        private readonly ITimeProvider _timeProvider;

        public UploadFileCommandHandler(IFileStorage fileStorage, IWritableApplicationContext context, ITimeProvider timeProvider)
        {
            _fileStorage = fileStorage;
            _context = context;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var instanceId = GenerateNewInstanceId();
            var dataType = Path.GetExtension(request.FormFile.FileName)?.Substring(1) ?? string.Empty;
            await CreateFile(instanceId, dataType, request.Tags, request.AuthorId, request.FormFile);
            return Unit.Value;
        }

        // it works as it should (placing throw new Ex before committing makes it rollback changes)
        private async Task CreateFile(string instanceId, string dataType, IEnumerable<string> tags, string authorId,
            IFormFile formFile)
        {
            await CreateFileInFileSystem(formFile, instanceId);
            using var transaction = _context.Database.BeginTransactionAsync();
            try
            {
                await CreateFileInDatabase(instanceId, dataType, tags, authorId);
                await transaction.Result.CommitAsync(CancellationToken.None);
            }
            catch (Exception)
            {
                await transaction.Result.RollbackAsync(CancellationToken.None);
                _fileStorage.Delete(instanceId);
                throw;
            }
        }

        private async Task CreateFileInDatabase(string id, string dataType, IEnumerable<string> tags, string authorId)
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

        private IEnumerable<Tag> ConvertStringsToTags(IEnumerable<string> stringTags)
        {
            var tagNames = _context.Tags.Select(t => t.Name);
            var tags = stringTags.Select(st =>
                tagNames.Contains(st) ? _context.Tags.Single(t => t.Name == st) : new Tag() { Name = st });

            return tags;
        }

        private async Task CreateFileInFileSystem(IFormFile formFile, string filename)
        {
            await using var stream = _fileStorage.CreateNew(filename);
            await formFile.CopyToAsync(stream);
        }

        private string GenerateNewInstanceId()
        {
            var idExists = true;
            var newId = string.Empty;
            var mediaInstancesIds = _context.MediaInstances
                .Select(mi => mi.Id)
                .ToList()
                .AsReadOnly();

            while (idExists)
            {
                newId = GenerateRandomBase32();

                if (!mediaInstancesIds.Contains(newId))
                {
                    idExists = false;
                }
            }

            return newId;
        }

        private string GenerateRandomBase32()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz2345678";
            var stringChars = new char[8];
            var random = new Random();

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new string(stringChars);
            return finalString;
        }
    }
}
