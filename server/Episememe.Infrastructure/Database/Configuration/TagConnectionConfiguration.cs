using Episememe.Domain.HelperEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Episememe.Infrastructure.Database.Configuration
{
    class TagConnectionConfiguration : IEntityTypeConfiguration<TagConnection>
    {
        public void Configure(EntityTypeBuilder<TagConnection> builder)
        {
            builder
                .HasOne(tc => tc.Successor)
                .WithMany()
                .HasForeignKey(tc => tc.SuccessorId);
            builder
                .HasOne(tc => tc.Ancestor)
                .WithMany()
                .HasForeignKey(tc => tc.AncestorId);

            builder.Property(tc => tc.AncestorId)
                .IsRequired();
            builder.Property(tc => tc.SuccessorId)
                .IsRequired();
            builder.Property(tc => tc.Hops)
                .IsRequired();
        }
    }
}
