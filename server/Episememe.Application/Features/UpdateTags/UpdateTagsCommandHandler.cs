using Episememe.Application.Exceptions;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Episememe.Application.Features.UpdateTags
{
    public class UpdateTagsCommandHandler : IRequestHandler<UpdateTagsCommand>
    {
        private readonly IWritableApplicationContext _context;
        private readonly ITimeProvider _timeProvider;

        public UpdateTagsCommandHandler(IWritableApplicationContext context, ITimeProvider timeProvider)
        {
            _context = context;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(UpdateTagsCommand request, CancellationToken cancellationToken)
        {

            var editedInstance = await _context.MediaInstances
                .Include(mi => mi.MediaTags)
                .SingleAsync(a => a.Id == request.Id, cancellationToken);

            if (editedInstance.IsPrivate && editedInstance.AuthorId != request.UserId)
                throw new MediaDoesNotBelongToUserException(request.UserId ?? string.Empty);

            ICollection<MediaTag> mediaTags = ConvertStringsToTags(request.Tags, editedInstance)
                .Select(t => new MediaTag()
                {
                    MediaInstance = editedInstance,
                    Tag = t
                }).ToList();

            editedInstance.MediaTags = mediaTags;
            await _context.SaveChangesAsync(cancellationToken);

            await CreateMediaChangeOnTagsUpdate(editedInstance.Id, request.UserId);

            return Unit.Value;
        }

        private IEnumerable<Tag> ConvertStringsToTags(IEnumerable<string> stringTags, MediaInstance media)
        {
            var tagNames = _context.Tags.Select(t => t.Name);
            var tags = stringTags.Select(st =>
                tagNames.Contains(st) ? _context.Tags.Single(t => t.Name == st) : new Tag() { Name = st});

            return tags;
        }

        private async Task CreateMediaChangeOnTagsUpdate(string mediaInstanceId, string userId)
        {
            var newMediaChange = new MediaChange
            {
                MediaInstanceId = mediaInstanceId,
                UserId = userId,
                Timestamp = _timeProvider.GetUtc(),
                Type = MediaChangeType.Update
            };

            await _context.MediaChanges.AddAsync(newMediaChange);
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}