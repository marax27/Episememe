using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class FileUploadDto
    {
        public IFormFile? FormFile { get; set; }
        public IEnumerable<string>? Tags { get; set; }
    }
}
