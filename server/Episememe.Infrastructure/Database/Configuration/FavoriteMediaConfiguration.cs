using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Episememe.Infrastructure.Database.Configuration
{
    class FavoriteMediaConfiguration : IEntityTypeConfiguration<FavoriteMedia>
    {
        public void Configure(EntityTypeBuilder<FavoriteMedia> builder)
        {
            builder
                .HasKey(fm => new { fm.MediaInstanceId, fm.UserId });
            builder
                .HasOne(fm => fm.MediaInstance)
                .WithMany(mi => mi.FavoriteMedia)
                .HasForeignKey(fm => fm.MediaInstanceId);
        }
    }
}
