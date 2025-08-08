using LinkNest.Api.Extenstions;
using System.Reflection;

namespace LinkNest.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            var app = builder.Build();
            app.RegisterPipelineFromAssembly(Assembly.GetExecutingAssembly());
            app.Run();
        }
    }
}
