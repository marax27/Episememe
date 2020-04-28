using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Episememe.Application.Tests.Helpers.Contexts
{
    interface IMediaInstanceDbSetTestContext
    {
        public DbSet<MediaInstance> MediaInstances { get; }
    }
}
