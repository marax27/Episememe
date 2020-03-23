using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Episememe.Infrastructure.Database
{
    public interface IDatabaseMigrationService
    {
        void Migrate();
    }
}
