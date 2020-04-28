using System;
using System.Collections.Generic;
using System.Text;

namespace Episememe.Application.DataTransfer
{
    public class LoadMediaDto
    {
        public string Id { get; } = null!;

        public LoadMediaDto(string id)
        {
            Id = id;
        }
    }
}
