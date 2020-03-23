using System.Reflection;
using System.Threading.Tasks;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using Microsoft.EntityFrameworkCore;

namespace Episememe.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext, IApplicationContext, IDatabaseMigrationService
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<MediaInstance> MediaInstances { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<MediaTag> MediaTags { get; set; } = null!;

        public void Migrate() => Database.Migrate();

        public void Update() => SaveChanges();

        public Task UpdateAsync() => SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
