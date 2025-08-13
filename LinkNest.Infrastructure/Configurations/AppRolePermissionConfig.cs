using LinkNest.Domain.Identity;
using LinkNest.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkNest.Infrastructure.Configurations
{
    internal class AppRolePermissionConfig : IEntityTypeConfiguration<AppRolePermission>
    {
        public void Configure(EntityTypeBuilder<AppRolePermission> builder)
        {
            builder.ToTable("RolePermissions");
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });
        }
    }
}
