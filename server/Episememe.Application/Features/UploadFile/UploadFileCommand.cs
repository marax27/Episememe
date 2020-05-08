using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Episememe.Application.Features.UploadFile
{
    public class UploadFileCommand : IRequest
    {
        public IFormFile FormFile { get; }
        public IEnumerable<string> Tags { get; }
        public string AuthorId { get; }

        private UploadFileCommand(IFormFile formFile, IEnumerable<string> tags, string authorId)
        {
            FormFile = formFile;
            Tags = tags;
            AuthorId = authorId;
        }

        public static UploadFileCommand Create(IFormFile formFile, IEnumerable<string> tags, string authorId)
        {
            return new UploadFileCommand(formFile, tags, authorId);
        }
    }
}
