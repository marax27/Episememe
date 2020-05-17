using MediatR;
using Episememe.Application.Interfaces;
using Episememe.Application.Exceptions;
using System.Linq;
using System.Collections.Generic;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using Microsoft.EntityFrameworkCore;
using System;
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
            MediaInstance editedInstance;
            try 
            {
                editedInstance = await _context.MediaInstances
                .Include(mi => mi.MediaTags)
                .SingleAsync(a => a.Id == request.Id);
            }
            catch (Exception){
                throw new FileDoesNotExistException("File not found");
            }

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