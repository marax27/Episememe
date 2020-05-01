using System.Threading;
using System.Threading.Tasks;

namespace Episememe.Application.Interfaces
{
    public interface IWritableContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
