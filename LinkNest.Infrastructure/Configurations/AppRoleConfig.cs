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
                .UsingEntity<AppRolePermission>();

            // or : and u don't have to define the AppRolePermission entity separately
            //.UsingEntity<AppRolePermission>(
            //   j => j
            //        .HasOne<AppPermission>()
            //        .WithMany()
            //        .HasForeignKey(rp => rp.PermissionId),
            //   j => j
            //        .HasOne<AppRole>()
            //        .WithMany()
            //        .HasForeignKey(rp => rp.RoleId),
            //   j =>
            //   {
            //       j.HasKey(rp => new { rp.RoleId, rp.PermissionId });
            //       j.ToTable("RolePermissions");
            //});
        }
    }
}
