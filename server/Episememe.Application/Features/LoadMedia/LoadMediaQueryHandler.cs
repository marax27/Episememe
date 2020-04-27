using Episememe.Application.DataTransfer;
using Episememe.Application.Interfaces;
using MediatR;
using System.IO;
using System.Web.Mvc;

namespace Episememe.Application.Features.LoadMedia
{
    public class LoadMediaQueryHandler : RequestHandler<LoadMediaQuery, LoadInstanceDto>
    {
        private IFileStorage _storage;

        public LoadMediaQueryHandler(IFileStorage storage)
            => _storage = storage;

        protected override LoadInstanceDto Handle(LoadMediaQuery request)
        {
            Stream stream = _storage.Read(request.LoadMedia.Id);
            string mimeType = "application/octet-stream";
            FileStreamResult file = new FileStreamResult(stream, mimeType)
            {
                FileDownloadName = request.LoadMedia.Id
            };
            return new LoadInstanceDto(request.LoadMedia.Id, file);
           
        }
    }
}