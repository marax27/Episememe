using Episememe.Application.DataTransfer;
using Episememe.Application.Interfaces;
using MediatR;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Episememe.Application.Features.LoadMedia
{
    public class LoadMediaQueryHandler : RequestHandler<LoadMediaQuery, IActionResult>
    {
        private IFileStorage _storage;

        public LoadMediaQueryHandler(IFileStorage storage)
            => _storage = storage;

        protected override IActionResult Handle(LoadMediaQuery request)
        {
            try{
                Stream stream = _storage.Read(request.LoadMedia.Id);
                string mimeType = "application/octet-stream";
                return new FileStreamResult(stream, mimeType)
                {
                FileDownloadName = request.LoadMedia.Id
                };
            }
            catch (ArgumentNullException ex){
                return new NotFoundResult();
            }
            
        }
    }
}