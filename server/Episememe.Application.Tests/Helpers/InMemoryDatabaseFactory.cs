using Episememe.Infrastructure.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Episememe.Application.Tests.Helpers
{
    public static class InMemoryDatabaseFactory
    {
        public static (ApplicationDbContext, DbConnection) CreateSqliteDbContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();

            return (context, connection);
        }
    }
}
