using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class TagsUpdateDto
    {
        public string? FileId { get; set; }
        public IEnumerable<string>? Tags { get; set; }
    }
}