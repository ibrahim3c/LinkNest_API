using LinkNest.Domain.Abstraction;
using LinkNest.Infrastructure.Services.DTOs;

namespace LinkNest.Application.Abstraction.IServices
{
    public interface IRoleService
    {
        Task<Result<IEnumerable<GetRoleDTO>>> GetAllRolesAsync();
        Task<Result<GetRoleDTO>> GetRoleByIdAsync(string roleId);
        Task<Result> AddRoleAsync(GetRoleDTO roleDTO);
        Task<Result> UpdateRoleAsync(GetRoleDTO roleDTO);
        Task<Result> DeleteRoleAsync(string roleId);

        Task<Result<IEnumerable<GetRoleDTO>>> GetRolesOfUserAsync(string userId);
        Task<Result<IEnumerable<string>>> GetRolesNameOfUserAsync(string userId);
        Task<Result<ManageRolesDTO>> GetRolesForManagingAsync(string userId);
        Task<Result> ManageUserRolesAsync(ManageRolesDTO manageRolesDTO);
    }
}
