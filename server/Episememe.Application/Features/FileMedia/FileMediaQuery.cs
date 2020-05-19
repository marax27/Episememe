using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Episememe.Application.Features.FileMedia
{
    public class FileMediaQuery : IRequest<IActionResult>
    {
        public string Id { get; }
        public string? UserId { get; }

        private FileMediaQuery(string id, string? userId)
        {
            Id = id;
            UserId = userId;
        }

        public static FileMediaQuery Create(string id, string? userId)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            return new FileMediaQuery(id, userId);
        }
    }
}
