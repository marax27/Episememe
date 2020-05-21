using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Episememe.Application.Features.FileMedia
{
    public class FileMediaQuery : IRequest<IActionResult>
    {
        public string Id { get; }
        public string Token { get; }

        private FileMediaQuery(string id, string token)
        {
            Id = id;
            Token = token;
        }

        public static FileMediaQuery Create(string id, string token)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            return new FileMediaQuery(id, token);
        }
    }
}
