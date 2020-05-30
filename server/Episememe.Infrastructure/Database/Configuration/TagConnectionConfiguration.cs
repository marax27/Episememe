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
                .HasKey(tc => new { tc.SuccessorId, tc.AncestorId, tc.Depth });
            builder
                .HasOne(tc => tc.Successor)
                .WithMany(tag => tag.Ancestors)
                .HasForeignKey(tc => tc.SuccessorId);
            builder
                .HasOne(tc => tc.Ancestor)
                .WithMany(tag => tag.Successors)
                .HasForeignKey(tc => tc.AncestorId);
            builder
                .Property(tc => tc.Depth)
                .IsRequired();
        }
    }
}
