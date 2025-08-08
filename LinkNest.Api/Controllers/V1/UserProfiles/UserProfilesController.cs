using Asp.Versioning;
using LinkNest.Application.UserProfiles.GetAllUserProfiles;
using LinkNest.Application.UserProfiles.GetUserProfile;
using LinkNest.Application.UserProfiles.UpdateUserProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LinkNest.Api.Controllers.V1.UserProfiles
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class UserProfilesController:ControllerBase
    {
        private readonly ISender sender;

        public UserProfilesController(ISender sender)
        {
            this.sender = sender;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserProfiles()
        {
            var query = new GetAllUserProfilesQuery();
            var result = await sender.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfileById(Guid id)
        {
            var query = new GetUserProfileQuery(id);
            var result = await sender.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfile( Guid id, UpdateUserProfileRequest request)
        {
            var command = new UpdateUserProfileCommand(id, request.FirstName, request.LastName, request.Email ,request.DateOfBirth, request.CurrentCity,request.PhoneNmber);

            var result = await sender.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}
