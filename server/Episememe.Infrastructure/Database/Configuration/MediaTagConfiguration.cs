using Episememe.Domain.HelperEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Episememe.Infrastructure.Database.Configuration
{
    class MediaTagConfiguration : IEntityTypeConfiguration<MediaTag>
    {
        public void Configure(EntityTypeBuilder<MediaTag> builder)
        {
            builder
                .HasKey(mt => new { mt.MediaInstanceId, mt.TagId });
            builder
                .HasOne(mt => mt.MediaInstance)
                .WithMany(mi => mi.MediaTags)
                .HasForeignKey(mt => mt.MediaInstanceId);
            builder
                .HasOne(mt => mt.Tag)
                .WithMany(t => t.MediaTags)
                .HasForeignKey(mt => mt.TagId);
        }
    }
}
