using LinkNest.Application.Abstraction.DTOs;
using LinkNest.Application.Abstraction.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LinkNest.Api.Controllers.V1.Roles
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController:ControllerBase
    {
        private readonly IRoleService roleService;

        public IPermissionService Permission { get; }

        public RolesController(IRoleService roleService, IPermissionService permission)
        {
            this.roleService = roleService;
            Permission = permission;
            this.roleService = roleService;
        }


        // roles operations
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var result = await roleService.GetAllRolesAsync();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetRoleById/{roleId}")]
        public async Task<IActionResult> GetRoleByIdAsync(string roleId)
        {
            var result = await roleService.GetRoleByIdAsync(roleId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] GetRoleDTO roleDTO)
        {
            var result = await roleService.AddRoleAsync(roleDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRoleAsync([FromBody] GetRoleDTO roleDTO)
        {
            var result = await roleService.UpdateRoleAsync(roleDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeleteRole/{roleId}")]
        public async Task<IActionResult> DeleteRoleAsync(string roleId)
        {
            var result = await roleService.DeleteRoleAsync(roleId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        // user Roles
        [HttpGet("GetRolesOfUser/{userId}")]
        public async Task<IActionResult> GetRolesOfUserAsync(string userId)
        {
            var result = await roleService.GetRolesOfUserAsync(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetRolesNameOfUser/{userId}")]
        public async Task<IActionResult> GetRolesNameOfUserAsync(string userId)
        {
            var result = await roleService.GetRolesNameOfUserAsync(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetRolesForManaging/{userId}")]
        public async Task<IActionResult> GetRolesForManagingAsync(string userId)
        {
            var result = await roleService.GetRolesForManagingAsync(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ManageUserRoles")]
        public async Task<IActionResult> ManageUserRolesAsync([FromBody] ManageRolesDTO manageRolesDTO)
        {
            var result = await roleService.ManageUserRolesAsync(manageRolesDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("AddPermissionToRole/{roleId}")]
        public async Task<IActionResult> AddPermissionToRoleAsync(string roleId, string permissionName)
        {
            var result = await Permission.AddPermissionToRoleAsync(roleId, permissionName);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("RemovePermissionFromRole/{roleId}")]
        public async Task<IActionResult> RemovePermissionFromRoleAsync(string roleId, string permissionName)
        {
            var result = await Permission.RemovePermissionFromRoleAsync(roleId, permissionName);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetPermissionsForManaging/{roleId}")]
        public async Task<IActionResult> GetPermissionsForManagingAsync(string roleId)
        {
            var result = await Permission.GetPermissionsForManagingAsync(roleId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("ManageUserPermissions")]
        public async Task<IActionResult> ManageUserPermissionsAsync([FromBody] ManagePermissionsDTO managePermissionsDTO)
        {
            var result= await Permission.ManageUserRolesAsync(managePermissionsDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



        }
}
