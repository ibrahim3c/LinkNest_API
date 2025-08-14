using LinkNest.Application.Abstraction.Helpers;
using LinkNest.Domain.Identity;
using LinkNest.Infrastructure.Auth;
using LinkNest.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

public class RolePermissionSeeder
{
    private readonly RoleManager<AppRole> roleManager;
    private readonly AppDbContext context;

    public RolePermissionSeeder(RoleManager<AppRole> roleManager, AppDbContext context)
    {
        this.roleManager = roleManager;
        this.context = context;
    }

    public async Task SeedAsync()
    {
        if (!context.Set<AppRolePermission>().Any())
        {
            // 1️⃣ Admin → All permissions
            await AssignPermissionsToRole(
                Roles.AdminRole,
                Enum.GetValues<Permission>().ToArray(),
                context
            );

            // 2️⃣ Moderator → Content & moderation permissions
            await AssignPermissionsToRole(
                Roles.ModeratorRole,
                new[]
                {
                    Permission.Post_Read,
                    Permission.Post_ReadAll,
                    Permission.Post_Create,
                    Permission.Post_Update,
                    Permission.Post_Delete,

                    Permission.PostComment_Read,
                    Permission.PostComment_Delete,

                    Permission.PostInteraction_Read,
                    Permission.PostInteraction_Delete,

                    Permission.UserProfile_Read,
                    Permission.UserProfile_ReadAll
                },
                context
            );

            // 3️⃣ User → Basic read/create permissions
            await AssignPermissionsToRole(
                Roles.UserRole,
                new[]
                {
                    Permission.Auth_Login,
                    Permission.Auth_Register,
                    Permission.Auth_RefreshToken,

                    Permission.UserProfile_Read,
                    Permission.UserProfile_Update,

                    Permission.Post_Read,
                    Permission.Post_ReadAll,
                    Permission.Post_Create,

                    Permission.PostComment_Create,
                    Permission.PostComment_Read,

                    Permission.PostInteraction_Create,
                    Permission.PostInteraction_Read,

                    Permission.Follow_Manage,
                    Permission.Follow_ReadFollowers,
                    Permission.Follow_ReadFollowees
                },
                context
            );

            await context.SaveChangesAsync();
        }
    }

    private async Task AssignPermissionsToRole(string roleName, Permission[] permissions, AppDbContext context)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null) return;

        var rolePermissions = permissions
            .Select(p => new AppRolePermission
            {
                RoleId = role.Id,
                PermissionId = (int)p
            }).ToList();

        await context.Set<AppRolePermission>().AddRangeAsync(rolePermissions);
    }
}
