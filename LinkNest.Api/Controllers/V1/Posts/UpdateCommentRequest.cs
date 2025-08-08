namespace LinkNest.Api.Controllers.V1.Posts
{
    public record UpdateCommentRequest(Guid commandId, string content);
}
