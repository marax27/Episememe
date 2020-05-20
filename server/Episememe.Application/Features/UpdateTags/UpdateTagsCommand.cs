using MediatR;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Episememe.Application.Features.UpdateTags
{
    public class UpdateTagsCommand : IRequest
    {
        public string Id { get; }
        public IEnumerable<string> Tags { get; }
        public string? UserId { get; }
    
        private UpdateTagsCommand(string id, IEnumerable<string> tags, string? userId)
        {
            Id = id;
            Tags = tags;
            UserId = userId;
        }

        public static UpdateTagsCommand Create(string? id, IEnumerable<string>? tags, string? userId)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (tags == null || !tags.Any())
                throw new ArgumentNullException(nameof(tags));
                
            return new UpdateTagsCommand(id, tags, userId);
        }
    }
}