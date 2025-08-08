using LinkNest.Domain.Follows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkNest.Infrastructure.Configurations
{
    internal sealed class FollowConfig : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            // Set table name (optional)
            builder.ToTable("Follows");

            // Configure key
            builder.HasKey(f => f.Guid);
            builder.Property(p => p.Guid)
                    .ValueGeneratedNever(); // Because you're assigning Guid manually

            // relationships
            builder.HasOne(f => f.Follower)
                      .WithMany(u => u.Following)
                      .HasForeignKey(f => f.FollowerId)
                      .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Followee)
                   .WithMany(u => u.Followers)
                   .HasForeignKey(f => f.FolloweeId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
