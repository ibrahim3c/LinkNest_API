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
        private readonly IOneSignalService oneSignalService;

        public TestController(IEmailService emailService,IOneSignalService oneSignalService)
        {
            this.emailService = emailService;
            this.oneSignalService = oneSignalService;
        }
        [HasPermission(Permission.Post_Read)]
        [HttpPost]
        public async Task<IActionResult> TestEmail()
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
