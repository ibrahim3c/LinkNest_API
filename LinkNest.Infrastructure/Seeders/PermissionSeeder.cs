using LinkNest.Domain.Identity;
using LinkNest.Infrastructure.Auth;
using LinkNest.Infrastructure.Data;

namespace LinkNest.Infrastructure.Seeders
{
    public class PermissionSeeder
    {
        private readonly AppDbContext context;

        public PermissionSeeder(AppDbContext context)
        {
            this.context = context;
        }
        public async Task SeedAsync()
        {
            if (!context.Set<AppPermission>().Any())
            {
                var perms = Enum.GetValues<Permission>()
                    .Select(p => new AppPermission
                    {
                        Id = (int)p,
                        Name = p.ToString()
                    })
                    .ToList();
                await context.Set<AppPermission>().AddRangeAsync(perms);
                await context.SaveChangesAsync();
            }
        }

    }
}
