using LinkNest.Application.Abstraction.IServices;
using LinkNest.Domain.Identity;
using LinkNest.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkNest.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext context;

        public PermissionService(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<HashSet<string>> GetPermissionsAsync(string userId)
        {
         //   var permissions = await (
         //    from user in context.Users
         //    join userRole in context.UserRoles on user.Id equals userRole.UserId
         //    join role in context.Roles on userRole.RoleId equals role.Id
         //    join rolePermission in context.Set<AppRolePermission>()
         //        on role.Id equals rolePermission.RoleId
         //    join permission in context.Set<AppPermission>()
         //        on rolePermission.PermissionId equals permission.Id
         //    where user.Id == userId
         //    select permission.Name
         //).ToListAsync();


        // or 
        var userRoles=await context.UserRoles.Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

            var perms = await context.Set<AppRolePermission>()
                .Where(rp => userRoles.Contains(rp.RoleId))
                .Include(rp => rp.Permission) 
                .Select(rp => rp.Permission.Name)
                .ToListAsync(); 

        return perms.ToHashSet();

        }
    }
}
