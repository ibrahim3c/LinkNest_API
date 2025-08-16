using LinkNest.Application.Abstraction.IServices;
using LinkNest.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LinkNest.Api.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController:ControllerBase
    {
        private readonly IEmailService emailService;

        public TestController(IEmailService emailService)
        {
            this.emailService = emailService;
        }
        [HasPermission(Permission.PostInteraction_Create)]
        [HttpPost]
        public async Task<IActionResult> Test()
        {
            try
            {

                await emailService.SendAsync("ihany941@gmail.com", "test", "for testing");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
