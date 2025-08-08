
using Asp.Versioning;

namespace LinkNest.Api.Registers
{
    public class ApiVersioningRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddApiVersioning(options =>
            {
                // if client does not add version => use default
                options.AssumeDefaultVersionWhenUnspecified = true;
                // the default=> 1.0
                options.DefaultApiVersion = new ApiVersion(1, 0);
                // show the version
                options.ReportApiVersions = true;
                // ways to expose version in api
                options.ApiVersionReader = ApiVersionReader.Combine(
                    // Query string: ?api-version=1.0
                    //new QueryStringApiVersionReader("api-version"),
                    // HTTP header: X-version: 1.0
                    new HeaderApiVersionReader("X-version")
                    // URL segment: /v1/controller
                    , new UrlSegmentApiVersionReader()
                //Media type parameter: Accept: application/json; ver=1.0
                //, new MediaTypeApiVersionReader("ver")
                );
                // for swagger
            }).AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
