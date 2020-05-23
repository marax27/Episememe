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
        private readonly IAuthorizationContext _authorization;

        public FileMediaQueryHandler(IFileStorage storage, IApplicationContext context, IAuthorizationContext authorization)
        {
            _storage = storage;
            _context = context;
            _authorization = authorization;
        }

        protected override IActionResult Handle(FileMediaQuery request)
        {
            var mediaInstance = _context.MediaInstances.Single(mi => mi.Id == request.Id);
            var userId = _authorization.BrowseTokens.Single(bt => bt.Id == request.Token).UserId;

            if (mediaInstance.IsPrivate && mediaInstance.AuthorId != userId)
                throw new MediaDoesNotBelongToUserException(request.Token ?? string.Empty);

            try
            {
                Stream stream = _storage.Read(request.Id);
                string mimeType = "application/octet-stream";
                return new FileStreamResult(stream, mimeType)
                {
                    EnableRangeProcessing = true,
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