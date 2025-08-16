namespace LinkNest.Application.Abstraction.DTOs
{
    public class ManagePermissionsDTO
    {
        public string RoleId { get; set; }
        public List<PermissionDTO> Permsissions { get; set; }
    }

    public class PermissionDTO
    {
        public string PermissionName { get; set; }
        public bool IsSelected { get; set; }

    }
}
