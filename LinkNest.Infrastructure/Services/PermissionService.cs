using LinkNest.Application.Abstraction.DTOs;
using LinkNest.Application.Abstraction.IServices;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Identity;
using LinkNest.Infrastructure.Data;
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

        // add permission to role 
        public async Task<Result> AddPermissionToRoleAsync(string roleId, string permissionName)
        {
            var role = await context.Set<AppRole>().FindAsync(roleId);
            if (role == null)
                return Result.Failure(["No Role Found"]);
            var permission = await context.Set<AppPermission>().FirstOrDefaultAsync(p => p.Name == permissionName);
            if (permission == null)
                return Result.Failure(["No Permission Found"]);
            var rolePermission = new AppRolePermission
            {
                RoleId = roleId,
                PermissionId = permission.Id
            };
            context.Set<AppRolePermission>().Add(rolePermission);
            await context.SaveChangesAsync();
            return Result.Success();
        }

        // remove permission from role
        public async Task<Result> RemovePermissionFromRoleAsync(string roleId, string permissionName)
        {
            var role = await context.Set<AppRole>().FindAsync(roleId);
            if (role == null)
                return Result.Failure(["No Role Found"]);
            var permission = await context.Set<AppPermission>().FirstOrDefaultAsync(p => p.Name == permissionName);
            if (permission == null)
                return Result.Failure(["No Permission Found"]);
            var rolePermission = await context.Set<AppRolePermission>()
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);
            if (rolePermission == null)
                return Result.Failure(["No Permission Assigned to this Role"]);
            context.Set<AppRolePermission>().Remove(rolePermission);
            await context.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result<ManagePermissionsDTO>> GetPermissionsForManagingAsync(string roleId)
        {
            var allPermissions = await context.Set<AppPermission>().Select(p=>p.Name).ToListAsync();

            var rolePermissions = await context.Set<AppRolePermission>()
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();
            var managePermissions = allPermissions.Select(p => new PermissionDTO
            {
                PermissionName = p,
                IsSelected = rolePermissions.Contains(p)
            }).ToList();

            var managePermissionsDTO = new ManagePermissionsDTO
            {
                RoleId = roleId,
                Permsissions = managePermissions
            };
            return Result<ManagePermissionsDTO>.Success(managePermissionsDTO);

        }
        public async Task<Result> ManageUserRolesAsync(ManagePermissionsDTO managePermissionsDTO)
        {
            var role = await context.Set<AppRole>().FindAsync(managePermissionsDTO.RoleId);
            if (role == null)
                return Result.Failure(["No role Found"]);

            var RolePermissions = await context.Set<AppRolePermission>()
                .Where(rp => rp.RoleId == managePermissionsDTO.RoleId)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();
            foreach (var permission in managePermissionsDTO.Permsissions)
            {
                if (RolePermissions.Any(rp => rp == permission.PermissionName) && !permission.IsSelected)
                    await RemovePermissionFromRoleAsync(role.Id, permission.PermissionName);

                if (!RolePermissions.Any(rp => rp == permission.PermissionName) && permission.IsSelected)
                    await AddPermissionToRoleAsync(role.Id, permission.PermissionName);
            }
            
            return Result.Success();
        }
    }
}
