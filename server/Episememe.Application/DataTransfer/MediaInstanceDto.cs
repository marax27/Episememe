using System;
using System.Collections.Generic;
using System.Text;

namespace Episememe.Application.DataTransfer
{
    public class MediaInstanceDto
    {
        public string Id { get; }
        public IEnumerable<string> Tags { get; }

        public MediaInstanceDto(string id, IEnumerable<string> tags)
        {
            Id = id;
            Tags = tags;
        }
    }
}
