using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

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
            ValidateId(id);
            return new FileMediaQuery(id);
        }

        protected static void ValidateId(string id){
            if (String.IsNullOrEmpty(id)){
                throw new ArgumentException();
            }
        }
        
    }
}
