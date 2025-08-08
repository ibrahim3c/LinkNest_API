namespace LinkNest.Api.Controllers.V1.Follows
{
    public record UnFollowRequest(Guid followeeId, Guid followerId);
}
