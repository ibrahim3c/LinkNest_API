using LinkNest.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Infrastructure.Configurations
{
    internal class AppRoleConfig : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasMany(r => r.Permissions)
                .WithMany()
                .UsingEntity<AppRolePermission>(
           j => j
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId),
           j => j
                .HasOne(rp => rp.Role)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId),
           j =>
           {
               j.HasKey(rp => new { rp.RoleId, rp.PermissionId });
               j.ToTable("RolePermissions");
           });

        }
    }
}
