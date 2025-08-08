using LinkNest.Domain.Posts;

namespace LinkNest.Api.Controllers.V1.Posts
{
    public record AddCommentRequest(string Content, Guid PostId, Guid UserProfileId);
}
