using LinkNest.Domain.Posts;
using LinkNest.Domain.UserProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkNest.Infrastructure.Configurations
{
    internal sealed class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Guid);

            builder.Property(p => p.Guid)
                .ValueGeneratedNever(); // Because you're assigning Guid manually

            builder.Property(p => p.Content)
                .HasMaxLength(1000)
                .IsRequired()
                .HasConversion(
                    c => c.content,
                    c => new Content(c)
                    );

            builder.Property(p => p.CreatedAt)
                   .IsRequired();

            builder.Property(p => p.ImageUrl)
                   .HasColumnName("ImageUrl")
                   .HasMaxLength(500)
                   .HasConversion(
                       u => u.url,
                       value => new Url(value)
                   );


            builder.Property(p => p.UserProfileId)
                   .IsRequired();

            // relationships
            builder.HasOne<UserProfile>()
                .WithMany(u=>u.Posts)
                .HasForeignKey(p => p.UserProfileId);

            //builder.HasMany(p => p.Comments)
            //       .WithOne()
            //       .HasForeignKey(c => c.PostId)
            //       .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(p => p.Interactions)
            //       .WithOne()
            //       .HasForeignKey(i => i.PostId)
            //       .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
