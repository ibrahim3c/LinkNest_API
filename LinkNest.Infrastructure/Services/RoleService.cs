using LinkNest.Application.Abstraction.DTOs;
using LinkNest.Application.Abstraction.IServices;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkNest.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RoleService(RoleManager<AppRole> roleManager,UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        // get
        public async Task<Result<IEnumerable<GetRoleDTO>>> GetAllRolesAsync()
        {
            var roles = await roleManager.Roles.Select(r => new GetRoleDTO
            {
                RoleId = r.Id,
                RoleName = r.Name
            }).ToListAsync();

            if (!roles.Any())
                return Result<IEnumerable<GetRoleDTO>>.Failure(["No Roles Found"]);

            return Result<IEnumerable<GetRoleDTO>>.Success(roles);
        }
        public async Task<Result<GetRoleDTO>> GetRoleByIdAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
                return Result<GetRoleDTO>.Failure(["No Role Found"]);

            var roleDTO = new GetRoleDTO { RoleId = roleId, RoleName = role.Name };

            return Result<GetRoleDTO>.Success(roleDTO);
        }
        // add
        public async Task<Result> AddRoleAsync(GetRoleDTO roleDTO)
        {
            if (await roleManager.FindByNameAsync(roleDTO.RoleName) != null)
            {
                return Result.Failure(["Role Name  already exists "]);
            }

            var role = new AppRole
            {
                Name = roleDTO.RoleName
            };

            IdentityResult result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
                return Result.Success();

            return Result.Failure(["Faild To Add this Role"]);
        }
        // update
        public async Task<Result> UpdateRoleAsync(GetRoleDTO roleDTO)
        {
            var role = await roleManager.FindByIdAsync(roleDTO.RoleId);
            if (role == null)
                return Result.Failure(["No Role Found"]);

            if (role.Name != roleDTO.RoleName)
            {
                role.Name = roleDTO.RoleName;
                IdentityResult result = await roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                    return Result.Failure(["Faild to update this Role"]);
            }

            return Result.Success();

        }
        // delete role
        public async Task<Result> DeleteRoleAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
                return Result.Failure(["No Role Found"]);

            IdentityResult result = await roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                return Result.Failure(["Faild to delete this Role"]);

            return Result.Success();

        }


        // User Roles
        public async Task<Result<IEnumerable<GetRoleDTO>>> GetRolesOfUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Result<IEnumerable<GetRoleDTO>>.Failure(["No User Found"]);

            var rolesNames = await userManager.GetRolesAsync(user);
            if (!rolesNames.Any())
                return Result<IEnumerable<GetRoleDTO>>.Failure(["No Roles Found"]);

            // if u want to get roles as object
            var roles = new List<GetRoleDTO>();
            foreach (var roleName in rolesNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    roles.Add(new GetRoleDTO
                    {
                        RoleId = role.Id,
                        RoleName = roleName,
                    });
                }
            }

            return Result<IEnumerable<GetRoleDTO>>.Success( roles);
        }
        public async Task<Result<IEnumerable<string>>> GetRolesNameOfUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Result<IEnumerable<string>>.Failure(["No User Found"]);

            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Any())
                return Result<IEnumerable<string>>.Failure(["No Roles Found"]);

            return Result<IEnumerable<string>>.Success( roles);
        }
        public async Task<Result<ManageRolesDTO>> GetRolesForManagingAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Result<ManageRolesDTO>.Failure(["No User Found"]);

            var roles = await GetAllRolesAsync();

            if (!roles.IsSuccess)
                return Result<ManageRolesDTO>.Failure(roles.Errors);

            var manageRoles = roles.Value.Select(r => new RolesDTO
            {
                RoleName = r.RoleName,
                IsSelected = userManager.IsInRoleAsync(user, r.RoleName).Result
            }).ToList();

            var UserRoles = new ManageRolesDTO
            {
                Roles = manageRoles,
                UserId = userId
            };
            return Result<ManageRolesDTO>.Success( UserRoles);

        }
        public async Task<Result> ManageUserRolesAsync(ManageRolesDTO manageRolesDTO)
        {
            var user = await userManager.FindByIdAsync(manageRolesDTO.UserId);
            if (user == null)
                return Result.Failure(["No User Found"]);
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in manageRolesDTO.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await userManager.AddToRoleAsync(user, role.RoleName);
            }
            return Result.Success();
        }

    }

}
