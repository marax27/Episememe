using Episememe.Application.Interfaces;
using Episememe.Application.Exceptions;
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