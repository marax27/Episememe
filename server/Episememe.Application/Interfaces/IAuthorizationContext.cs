using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Episememe.Application.Interfaces
{
    public interface IAuthorizationContext : IWritableContext
    {
        DbSet<BrowseToken> BrowseTokens { get; }
    }
}
