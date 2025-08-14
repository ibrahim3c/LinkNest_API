namespace LinkNest.Application.Abstraction.IServices
{
    public interface IPermissionService
    {
        Task<HashSet<string>>GetPermissionsAsync(string userId);
    }
}
