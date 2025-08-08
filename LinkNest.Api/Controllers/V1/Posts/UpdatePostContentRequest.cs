using LinkNest.Domain.Posts;

namespace LinkNest.Api.Controllers.V1.Posts
{
    public record UpdatePostContentRequest(Guid postId, string Content);

}
