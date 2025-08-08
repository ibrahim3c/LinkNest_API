
using LinkNest.Api.Middlewares;

namespace LinkNest.Api.Registers
{
    public class MiddlewareRegistrar : IWebApplicationRegistrar
    {
        public void RegisterPipelineComponents(WebApplication app)
        {

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            //app.UseMiddleware<GlobalExceptionHandler>();
            app.UseAuthorization();


            app.MapControllers();
        }
    }
}
