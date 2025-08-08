
using LinkNest.Application;
using LinkNest.Application.Abstraction.Helpers;
using LinkNest.Infrastructure;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LinkNest.Api.Registers
{
    public class DependencyInjectionRegisterar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            //Add configuration from the secret.json file
            builder.Configuration.AddJsonFile("Secret.json", optional: false, reloadOnChange: true);

            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructure(builder.Configuration);
            //serilog;
            builder.Host.UseSerilog((context, config) =>
            {                              // read configs from appsettigns
                config.ReadFrom.Configuration(context.Configuration);
            });
            //sendGrid
            builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGridSettings")); 


        }
    }
}
