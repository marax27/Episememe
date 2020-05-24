using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class FileUploadFormData
    {
        public IFormFile? File { get; set; }
        public string? Media { get; set; }
    }
}
