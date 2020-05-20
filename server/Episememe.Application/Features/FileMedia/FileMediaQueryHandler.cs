using Episememe.Application.Exceptions;
using Episememe.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace Episememe.Application.Features.FileMedia
{
    public class FileMediaQueryHandler : RequestHandler<FileMediaQuery, IActionResult>
    {
        private readonly IFileStorage _storage;
        private readonly IApplicationContext _context;

        public FileMediaQueryHandler(IFileStorage storage, IApplicationContext context)
        {
            _storage = storage;
            _context = context;
        }

        protected override IActionResult Handle(FileMediaQuery request)
        {
            var mediaInstance = _context.MediaInstances.Single(mi => mi.Id == request.Id);

            if (mediaInstance.IsPrivate && mediaInstance.AuthorId != request.UserId)
                throw new MediaDoesNotBelongToUserException(request.UserId ?? string.Empty);

            try
            {
                Stream stream = _storage.Read(request.Id);
                string mimeType = "application/octet-stream";
                return new FileStreamResult(stream, mimeType)
                {
                    FileDownloadName = request.Id
                };
            }
            catch (FileDoesNotExistException)
            {
                return new NotFoundResult();
            }
        }
    }
}