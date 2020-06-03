using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Episememe.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext, IApplicationContext, IAuthorizationContext, IWritableApplicationContext, IDatabaseMigrationService
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<MediaInstance> MediaInstances { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<MediaTag> MediaTags { get; set; } = null!;
        public DbSet<FavoriteMedia> FavoriteMedia { get; set; } = null!;

        public DbSet<BrowseToken> BrowseTokens { get; set; } = null!;
        public DbSet<MediaChange> MediaChanges { get; set; } = null!;

        public void Migrate() => Database.Migrate();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
