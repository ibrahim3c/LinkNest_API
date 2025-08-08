
namespace LinkNest.Api.Registers
{
    public class SwaggerRegistrar : IWebApplicationBuilderRegistrar{

        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen();
        }
    }
}
