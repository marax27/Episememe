using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Episememe.Application.Tests.Helpers.Contexts
{
    interface IDbSetTestContext<T> where T : class
    {
        public DbSet<T> Instances { get; }
    }
}
