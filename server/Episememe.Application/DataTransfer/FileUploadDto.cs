using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class FileUploadDto
    {
        public IFormFile? File { get; set; }
        public string? Tags { get; set; }
        public bool IsPrivate { get; set; }
    }
}
