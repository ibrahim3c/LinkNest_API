using ApartmentBooking.Domain.Users;
using LinkNest.Domain.Identity;
using LinkNest.Domain.UserProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkNest.Infrastructure.Configurations
{
    internal sealed class UserProfileCofig : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(u => u.Guid);

            builder.Property(u => u.Guid)
                   .ValueGeneratedNever();

            // Map value objects as properties with conversion
            builder.Property(u => u.FirstName)
                   .HasConversion(
                       fn => fn.firstname,
                       value => new FirstName(value)
                   )
                   .HasMaxLength(100)
                   .HasColumnName("FirstName");

            builder.Property(u => u.LastName)
                   .HasConversion(
                       ln => ln.lastname,
                       value => new LastName(value)
                   )
                   .HasMaxLength(100)
                   .HasColumnName("LastName");

            builder.Property(u => u.Email)
                   .HasConversion(
                       e => e.email,
                       value => new UserProfileEmail(value)
                   )
                   .HasMaxLength(200)
                   .HasColumnName("Email");

            builder.Property(u => u.CurrentCity)
                   .HasConversion(
                       c => c.currentCity,
                       value => new CurrentCity(value)
                   )
                   .HasMaxLength(100)
                   .HasColumnName("CurrentCity");

            builder.Property(u => u.DateOfBirth)
                   .IsRequired();

            builder.Property(u => u.CreatedOn)
                   .IsRequired();

            builder.HasOne(u => u.AppUser)
                    .WithOne()
                    .HasForeignKey<UserProfile>(up => up.AppUserId);

        }
    }
}
