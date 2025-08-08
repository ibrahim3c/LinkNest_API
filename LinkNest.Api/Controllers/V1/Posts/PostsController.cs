using Asp.Versioning;
using LinkNest.Application.Follows.GetAllFollowees;
using LinkNest.Application.Posts.AddCommentToPost;
using LinkNest.Application.Posts.AddInteractionToPost;
using LinkNest.Application.Posts.AddPost;
using LinkNest.Application.Posts.DeleteCommentToPost;
using LinkNest.Application.Posts.DeleteInteractionToPost;
using LinkNest.Application.Posts.DeletePost;
using LinkNest.Application.Posts.GetPost;
using LinkNest.Application.Posts.GetPostComments;
using LinkNest.Application.Posts.GetPostInteractions;
using LinkNest.Application.Posts.GetUserPosts;
using LinkNest.Application.Posts.UpdateCommentToPost;
using LinkNest.Application.Posts.UpdatePostContent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LinkNest.Api.Controllers.V1.Posts
{
    [ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class PostsController:ControllerBase
    {
        private readonly ISender sender;

        public PostsController(ISender sender)
        {
            this.sender = sender;
        }

        // GET: api/users/{userId}/posts
        [HttpGet("{userId}/posts")]
        public async Task<IActionResult> GetUserPosts(Guid userId)
        {
            var query=new GetUserPostsQuery(userId);
            var result=await sender.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);

        }

        // GET: api/posts/{postId}
        [HttpGet("/api/posts/{postId:guid}")]
        public async Task<IActionResult> GetPost(Guid postId)
        {
            var query = new GetPostQuery(postId);
            var result = await sender.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

                return Ok(result);

        }

        // GET: api/posts/{postId}/interactions
        [HttpGet("/api/posts/{postId:guid}/interactions")]
        public async Task<IActionResult> GetPostInteractions(Guid postId)
        {
            var query = new GetPostInteractionsQuery(postId);
            var result = await sender.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        // GET: api/posts/{postId}/comments
        [HttpGet("/api/posts/{postId:guid}/comments")]
        public async Task<IActionResult> GetPostComments(Guid postId)
        {
            var query = new GetPostCommentsQuery(postId);
            var result = await sender.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result);
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddPost([FromBody] AddPostRequest addPostRequest)
        {
            var command = new AddPostCommand(addPostRequest.Content, addPostRequest.ImageUrl, addPostRequest.UserProfileId);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        [HttpPost("comment")]
        public async Task<IActionResult> AddCommentToPost([FromBody] AddCommentRequest addCommentRequest)
        {
            var command = new AddCommentCommand(addCommentRequest.Content, addCommentRequest.PostId, addCommentRequest.UserProfileId);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpPost("interaction")]
        public async Task<IActionResult> AddInteractionoPost([FromBody] AddInteractionRequest addInteractionRequest)
        {
            var command = new AddInteractionCommand(addInteractionRequest.postId, addInteractionRequest.userProfileId, addInteractionRequest.interactionType);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePostContent([FromBody] UpdatePostContentRequest updatePostContentRequest)
        {
            var command = new UpdatePostContentCommand(updatePostContentRequest.postId, updatePostContentRequest.Content);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpPut("comment/update")]
        public async Task<IActionResult> UpdateCommentContent([FromBody] UpdateCommentRequest updateCommentRequest)
        {
            var command = new UpdateCommentCommand(updateCommentRequest.commandId, updateCommentRequest.content);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }



        [HttpDelete("{postId:guid}")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            var command = new DeletePostCommand(postId);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpDelete("comment/{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var command = new DeleteCommentCommand(commentId);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }

        [HttpDelete("interaction/{interactionId:guid}")]
        public async Task<IActionResult> DeleteInteraction(Guid interactionId)
        {
            var command = new DeleteInteractionCommand(interactionId);
            var result = await sender.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);
            return Ok(result);
        }


    }
}
