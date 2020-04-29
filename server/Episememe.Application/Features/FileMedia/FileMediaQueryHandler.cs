using Episememe.Application.Interfaces;
using Episememe.Application.Exceptions;
using MediatR;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Episememe.Application.Features.FileMedia
{
    public class FileMediaQueryHandler : RequestHandler<FileMediaQuery, IActionResult>
    {
        private IFileStorage _storage;

        public FileMediaQueryHandler(IFileStorage storage)
            => _storage = storage;

        protected override IActionResult Handle(FileMediaQuery request)
        {
            try{
                Stream stream = _storage.Read(request.Id);
                string mimeType = "application/octet-stream";
                return new FileStreamResult(stream, mimeType)
                {
                FileDownloadName = request.Id
                };
            }
            catch (FileDoesNotExistException ex){
                return new NotFoundResult();
            }
        }
    }
}