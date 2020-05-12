using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Episememe.Application.Features.FileUpload
{
    public class FileUploadCommand : IRequest
    {
        public IFormFile FormFile { get; }
        public IEnumerable<string> Tags { get; }
        public string AuthorId { get; }

        private FileUploadCommand(IFormFile formFile, IEnumerable<string> tags, string authorId)
        {
            FormFile = formFile;
            Tags = tags;
            AuthorId = authorId;
        }

        public static FileUploadCommand Create(IFormFile formFile, IEnumerable<string> tags, string authorId)
        {
            return new FileUploadCommand(formFile, tags, authorId);
        }
    }
}
