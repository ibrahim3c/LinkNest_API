using LinkNest.Domain.Identity;
using LinkNest.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkNest.Infrastructure.Configurations
{
    internal class AppPermissionCofig : IEntityTypeConfiguration<AppPermission>
    {
        public void Configure(EntityTypeBuilder<AppPermission> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(256);

            var perms = Enum.GetValues<Permission>()
                .Select(p => new AppPermission
                {
                    Id = (int)p,
                    Name = p.ToString()
                });

            builder.HasData(perms);
        }
    }
}
