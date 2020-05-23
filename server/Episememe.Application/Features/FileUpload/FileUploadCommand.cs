using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using Episememe.Application.DataTransfer;

namespace Episememe.Application.Features.FileUpload
{
    public class FileUploadCommand : IRequest
    {
        public IFormFile FormFile { get; }
        public FileUploadDto MediaDto { get; }
        public string? AuthorId { get; }

        private FileUploadCommand(IFormFile formFile, FileUploadDto mediaDto, string? authorId)
        {
            FormFile = formFile;
            MediaDto = mediaDto;
            AuthorId = authorId;
        }

        public static FileUploadCommand Create(IFormFile? formFile, FileUploadDto mediaDto, string? authorId)
        {
            if (formFile == null)
                throw new ArgumentNullException(nameof(formFile));
            if (mediaDto == null)
                throw new ArgumentNullException(nameof(mediaDto));
            if (mediaDto.Tags == null)
                throw new ArgumentNullException(nameof(mediaDto.Tags));

            return new FileUploadCommand(formFile, mediaDto, authorId);
        }
    }
}
