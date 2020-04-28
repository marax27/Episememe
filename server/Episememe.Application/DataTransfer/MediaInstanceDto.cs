using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class MediaInstanceDto
    {
        public string Id { get; }
        public string DataType { get; }
        public IEnumerable<string> Tags { get; }

        public MediaInstanceDto(string id, string dataType, IEnumerable<string> tags)
        {
            Id = id;
            DataType = dataType;
            Tags = tags;
        }
    }
}
