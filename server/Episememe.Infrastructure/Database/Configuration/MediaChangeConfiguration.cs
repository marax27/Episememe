using Episememe.Domain.HelperEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Episememe.Infrastructure.Database.Configuration
{
    class MediaChangeConfiguration : IEntityTypeConfiguration<MediaChange>
    {
        public void Configure(EntityTypeBuilder<MediaChange> builder)
        {
            builder.Property(mc => mc.MediaInstanceId)
                .IsRequired();
            builder.Property(mc => mc.Timestamp)
                .IsRequired();
            builder.Property(mc => mc.Type)
                .IsRequired()
                .HasConversion<string>();

            builder
                .HasOne(mc => mc.MediaInstance)
                .WithMany(mi => mi.MediaChanges)
                .HasForeignKey(mc => mc.MediaInstanceId);
        }
    }
}
