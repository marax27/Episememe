using MediatR;
using System;
using System.Collections.Generic;

namespace Episememe.Application.Features.UpdateTags
{
    public class UpdateTagsCommand : IRequest
    {
        public string Id { get; }
        public IEnumerable<string> Tags { get; }
    
        private UpdateTagsCommand(string id, IEnumerable<string> tags)
        {
            Id = id;
            Tags = tags;
        }

        public static UpdateTagsCommand Create(string id, IEnumerable<string> tags)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (tags == null)
                throw new ArgumentNullException(nameof(tags));
            return new UpdateTagsCommand(id, tags);
        }
    }
}