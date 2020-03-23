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

        public DbSet<MediaInstance> MediaInstances { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<MediaTag> MediaTags { get; set; }

        public void Migrate() => Database.Migrate();

        public void Update() => SaveChanges();

        public Task UpdateAsync() => SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaTag>()
                .HasKey(mt => new { mt.MediaInstanceId, mt.TagId });
            modelBuilder.Entity<MediaTag>()
                .HasOne(mt => mt.MediaInstance)
                .WithMany(mi => mi.MediaTags)
                .HasForeignKey(mt => mt.MediaInstanceId);
            modelBuilder.Entity<MediaTag>()
                .HasOne(mt => mt.Tag)
                .WithMany(t => t.MediaTags)
                .HasForeignKey(mt => mt.TagId);
        }
    }
}
