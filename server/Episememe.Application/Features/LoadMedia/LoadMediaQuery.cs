using Episememe.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Episememe.Application.Features.LoadMedia
{
    public class LoadMediaQuery : IRequest<IActionResult>
    {
        public string Id { get; }

        private LoadMediaQuery(string id)
        {
            Id = id;
        }

        public static LoadMediaQuery Create(string id)
        {
            return new LoadMediaQuery(id);
        }
    }
}
