using LinkNest.Application.Abstraction.DTOs;
using LinkNest.Domain.Abstraction;

namespace LinkNest.Application.Abstraction.IServices
{
    public interface IPermissionService
    {
        Task<HashSet<string>>GetPermissionsAsync(string userId);
        Task<Result> AddPermissionToRoleAsync(string roleId, string permissionName);
        Task<Result> RemovePermissionFromRoleAsync(string roleId, string permissionName);
        Task<Result<ManagePermissionsDTO>> GetPermissionsForManagingAsync(string roleId);
        Task<Result> ManageUserRolesAsync(ManagePermissionsDTO managePermissionsDTO);
    }
}
