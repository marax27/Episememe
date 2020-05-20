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

        public UpdateTagsCommandHandler(IWritableApplicationContext context)
            => _context = context;

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

            return Unit.Value;
        }

        private IEnumerable<Tag> ConvertStringsToTags(IEnumerable<string> stringTags, MediaInstance media)
        {
            var tagNames = _context.Tags.Select(t => t.Name);
            var tags = stringTags.Select(st =>
                tagNames.Contains(st) ? _context.Tags.Single(t => t.Name == st) : new Tag() { Name = st, Description = st });

            return tags;
        }

    }
}