using LinkNest.Api.Extenstions;
using LinkNest.Domain.Identity;
using LinkNest.Infrastructure.Data;
using LinkNest.Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace LinkNest.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            var app = builder.Build();
            app.RegisterPipelineFromAssembly(Assembly.GetExecutingAssembly());

            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    // seed permissions
            //    var permissionSeeder = new PermissionSeeder(
            //        services.GetRequiredService<AppDbContext>()
            //    );
            //    await permissionSeeder.SeedAsync();

            //    var RolePermissionSeeder = new RolePermissionSeeder(
            //        services.GetRequiredService<RoleManager<AppRole>>(),
            //        services.GetRequiredService<AppDbContext>()
            //    );
            //    await RolePermissionSeeder.SeedAsync();
            //}

            app.Run();
        }
    }
}
