using Episememe.Domain.HelperEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Episememe.Infrastructure.Database.Configuration
{
    class TagConnectionConfiguration : IEntityTypeConfiguration<TagConnection>
    {
        public void Configure(EntityTypeBuilder<TagConnection> builder)
        {
            builder.Property(tc => tc.Ancestor)
                .IsRequired();
            builder.Property(tc => tc.Successor)
                .IsRequired();
            builder.Property(tc => tc.Hops)
                .IsRequired();
        }
    }
}
