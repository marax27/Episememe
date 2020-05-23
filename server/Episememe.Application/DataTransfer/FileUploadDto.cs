using System;
using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class FileUploadDto
    {
        public IEnumerable<string> Tags { get; }
        public DateTime Timestamp { get; }
        public bool IsPrivate { get; }

        public FileUploadDto(IEnumerable<string> tags, DateTime timestamp, bool isPrivate)
        {
            Tags = tags;
            Timestamp = timestamp;
            IsPrivate = isPrivate;
        }
    }
}
