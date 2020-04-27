using System;
using System.Collections.Generic;
using System.Text;

namespace Episememe.Application.DataTransfer
{
    public class LoadMediaDto
    {
        public string Id { get; }

        public LoadMediaDto(string id)
        {
            Id = id;
        }
    }
}
