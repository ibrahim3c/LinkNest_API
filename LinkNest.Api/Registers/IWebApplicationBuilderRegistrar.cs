namespace LinkNest.Api.Registers
{
    // Used to register services(AddXyz()) into the WebApplicationBuilder.
    public interface IWebApplicationBuilderRegistrar
    {
        void RegisterServices(WebApplicationBuilder builder);
    }
}
