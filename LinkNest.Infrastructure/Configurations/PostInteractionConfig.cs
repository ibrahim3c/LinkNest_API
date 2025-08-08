using LinkNest.Domain.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkNest.Infrastructure.Configurations
{
    internal sealed class PostInteractionConfig : IEntityTypeConfiguration<PostInteraction>
    {
        public void Configure(EntityTypeBuilder<PostInteraction> builder)
        {
            builder.HasKey(pi => pi.Guid);

            builder.Property(pi => pi.Guid)
                   .ValueGeneratedNever(); // because you're assigning Guid manually

            builder.Property(pi => pi.PostId)
                   .IsRequired();

            builder.Property(pi => pi.UserProfileId)
                   .IsRequired();

            builder.Property(pi => pi.CreatedAt)
                   .IsRequired();

            
            builder.Property(pi => pi.InteractionType)
                   .HasConversion(
                    pi=>(int)pi,
                    pi=>(InteractionTypes)pi
                   ) 
                   .HasColumnName("InteractionType")
                   .IsRequired();

            // Relationships
            builder.HasOne(pi => pi.Post)
                   .WithMany(p => p.Interactions)
                   .HasForeignKey(pi => pi.PostId)
                   .OnDelete(DeleteBehavior.Cascade); // optional
        }
    }
}
