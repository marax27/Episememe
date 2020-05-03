using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Episememe.Infrastructure.Database.Configuration
{
    class BrowseTokenConfiguration : IEntityTypeConfiguration<BrowseToken>
    {
        public void Configure(EntityTypeBuilder<BrowseToken> builder)
        {
            builder.Property(bt => bt.ExpirationTime)
                .IsRequired();
        }
    }
}
