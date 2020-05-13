using MediatR;
using Episememe.Application.Interfaces;
using System.Linq;
using System.Collections.Generic;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;



namespace Episememe.Application.Features.UpdateTags
{
    public class UpdateTagsCommandHandler : RequestHandler<UpdateTagsCommand>
    {
        private readonly IWritableApplicationContext _context;

        public UpdateTagsCommandHandler(IWritableApplicationContext context)
            => _context = context;

        protected override void Handle(UpdateTagsCommand request)
        {
            MediaInstance editedInstance;
            try 
            {
                editedInstance = _context.MediaInstances.First(a => a.Id == request.Id);
            }
            catch (Exception){
                throw;
            }

            ICollection<MediaTag> mediaTags = ConvertStringsToTags(request.Tags, editedInstance)
            .Select(t => new MediaTag()
            {
                MediaInstance = editedInstance,
                Tag = t
            }).ToList();
            editedInstance.MediaTags = mediaTags;
            _context.SaveChanges();
        }

        private IEnumerable<Tag> ConvertStringsToTags(IEnumerable<string> stringTags, MediaInstance media)
        {
            var tagNames = _context.Tags.Select(t => t.Name);
            var mediaTagNames = media.MediaTags.Select(t => t.Tag.Name);
            stringTags = stringTags.Where(t => !(mediaTagNames.Contains(t)));
            var tags = stringTags.Select(st =>
                tagNames.Contains(st) ? _context.Tags.Single(t => t.Name == st) : new Tag() { Name = st, Description = st });

            return tags;
        }

    }
}