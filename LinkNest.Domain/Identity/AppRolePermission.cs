namespace LinkNest.Domain.Identity
{
    public class AppRolePermission
    {
        public string RoleId { get; set; }
        public AppRole Role { get; set; }

        public int PermissionId { get; set; }
        public AppPermission Permission { get; set; }
    }
}
