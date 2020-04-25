using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Episememe.Application.Tests.Helpers.Contexts
{
    interface IMediaInstanceDbSetTestContext
    {
        public DbSet<MediaInstance> MediaInstances { get; }
    }
}
