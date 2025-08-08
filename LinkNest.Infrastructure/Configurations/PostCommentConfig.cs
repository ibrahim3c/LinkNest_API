using LinkNest.Domain.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkNest.Infrastructure.Configurations
{
    internal sealed class PostCommentConfig : IEntityTypeConfiguration<PostComment>
    {
        public void Configure(EntityTypeBuilder<PostComment> builder)
        {
            builder.HasKey(pc => pc.Guid);

            builder.Property(pc => pc.Guid)
                   .ValueGeneratedNever(); // because you're assigning the Guid manually

            // if complex value object (has more than one prop) use Owns
            // but simpel value object  (has just one prop) use property and HasConversion

            // Map value object `Content`
            builder.Property(pc => pc.Content)
                   .HasConversion(
                       c => c.content,               // To DB
                       value => new Content(value) // From DB
                   )
                   .HasMaxLength(1000)
                   .HasColumnName("Content");

            builder.Property(pc => pc.CreatedAt)
                   .IsRequired();

            builder.Property(pc => pc.PostId)
                   .IsRequired();

            builder.Property(pc => pc.UserProfileId)
                   .IsRequired();

            // Relationships
            builder.HasOne(pc => pc.Post)
                   .WithMany(p => p.Comments)
                   .HasForeignKey(pc => pc.PostId)
                   .OnDelete(DeleteBehavior.Cascade); // Or as needed
        }
    }
}
