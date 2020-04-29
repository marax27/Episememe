using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Episememe.Application.Features.FileMedia
{
    public class FileMediaQuery : IRequest<IActionResult>
    {
        public string Id { get; }

        private FileMediaQuery(string id)
        {
            Id = id;
        }

        public static FileMediaQuery Create(string id)
        {
            return new FileMediaQuery(id);
        }
    }
}
