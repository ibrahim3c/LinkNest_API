using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace LinkNest.Api.Controllers.V2
{
    [ApiController]
    //[Route("posts/v{version:apiVersion}/[controller]")]
    [Route("posts/[controller]")]
    [ApiVersion("2.0")]
    public class PostsController:ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            return Ok("this  from V2");
        }
    }
}
