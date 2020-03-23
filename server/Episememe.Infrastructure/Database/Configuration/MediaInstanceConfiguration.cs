using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Episememe.Infrastructure.Database.Configuration
{
    class MediaInstanceConfiguration : IEntityTypeConfiguration<MediaInstance>
    {
        public void Configure(EntityTypeBuilder<MediaInstance> builder)
        {
            builder.Property(mi => mi.Timestamp)
                .IsRequired();
            builder.Property(mi => mi.DataType)
                .IsRequired();
            builder.Property(mi => mi.Path)
                .IsRequired();
            builder.Property(mi => mi.RevisionCount)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}
