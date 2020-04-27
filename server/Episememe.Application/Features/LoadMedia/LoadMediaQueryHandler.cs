using Episememe.Application.DataTransfer;
using Episememe.Application.Interfaces;
using MediatR;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Episememe.Application.Features.LoadMedia
{
    public class LoadMediaQueryHandler : RequestHandler<LoadMediaQuery, FileStreamResult>
    {
        private IFileStorage _storage;

        public LoadMediaQueryHandler(IFileStorage storage)
            => _storage = storage;

        protected override FileStreamResult Handle(LoadMediaQuery request)
        {
            Stream stream = _storage.Read(request.LoadMedia.Id);
            string mimeType = "application/octet-stream";
            return new FileStreamResult(stream, mimeType)
            {
                FileDownloadName = request.LoadMedia.Id
            };
        }
    }
}