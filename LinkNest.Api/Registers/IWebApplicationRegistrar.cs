namespace LinkNest.Api.Registers
{
    //Used to register middleware (UseXyz()) into the WebApplication.
    public interface IWebApplicationRegistrar
    {
        void RegisterPipelineComponents(WebApplication app);

    }
}
