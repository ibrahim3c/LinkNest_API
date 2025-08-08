using Asp.Versioning;
using LinkNest.Application.Follows.FollowUserProfile;
using LinkNest.Application.Follows.GetAllFollowees;
using LinkNest.Application.Follows.GetAllFollowers;
using LinkNest.Application.Follows.UnFollowUserProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LinkNest.Api.Controllers.V1.Follows
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class FollowsController:ControllerBase
    {
        private readonly ISender sender;

        public FollowsController(ISender sender)
        {
            this.sender = sender;
        }
        // GET: api/users/{followerId}/following
        [HttpGet("{followerId}/following")]
        public async Task<IActionResult> GetAllFollowing(Guid followerId)
        {
            var query = new GetAllFolloweesQuery(followerId);
            var result = await sender.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        // GET: api/users/{followeeId}/followers
        [HttpGet("{followeeId}/followers")]
        public async Task<IActionResult> GetAllFollowers(Guid followeeId)
        {
            var query = new GetAllFollowersQuery(followeeId);
            var result = await sender.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpPost("follow")]
        public async Task<IActionResult> Follow(FollowRequest followRequest)
        {
            var command = new FollowCommand(followRequest.followeeId,followRequest.followingId);
            var result = await sender.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpPost("unfollow")]
        public async Task<IActionResult> UnFollow(UnFollowRequest unFollowRequest)
        {
            var command = new UnFollowCommand(unFollowRequest.followeeId, unFollowRequest.followerId);
            var result = await sender.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

    }
}
