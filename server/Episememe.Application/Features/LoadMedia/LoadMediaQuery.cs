using Episememe.Application.DataTransfer;
using Episememe.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Episememe.Application.Features.LoadMedia
{
    public class LoadMediaQuery : IRequest<FileStreamResult>
    {
        public LoadMediaDto LoadMedia { get; }

        private LoadMediaQuery(LoadMediaDto loadMedia)
        {
            LoadMedia = loadMedia;
        }

        public static LoadMediaQuery Create(LoadMediaDto loadMedia)
        {
            return new LoadMediaQuery(loadMedia);
        }
    }
}
