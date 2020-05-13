using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Episememe.Application.Interfaces
{
    public interface IWritableApplicationContext : IApplicationContext, IWritableContext
    {
        DatabaseFacade Database { get; }
    }
}
