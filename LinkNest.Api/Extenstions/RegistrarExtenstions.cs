using LinkNest.Api.Registers;
using System.Reflection;

namespace LinkNest.Api.Extenstions
{
    public static class RegistrarExtenstions
    {
        public static void RegisterServicesFromAssembly(this WebApplicationBuilder builder, Assembly assembly)
        {
            var registers = assembly
            .GetTypes()
                .Where(t => typeof(IWebApplicationBuilderRegistrar).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(Activator.CreateInstance)
                .Cast<IWebApplicationBuilderRegistrar>();

            foreach (var register in registers)
            {
                register.RegisterServices(builder);
            }
        }

        public static void RegisterPipelineFromAssembly(this WebApplication app, Assembly assembly)
        {
            var registers = assembly
                .GetTypes()
                .Where(t => typeof(IWebApplicationRegistrar).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IWebApplicationRegistrar>();

            foreach (var register in registers)
            {
                register.RegisterPipelineComponents(app);
            }
        }
    }
}
