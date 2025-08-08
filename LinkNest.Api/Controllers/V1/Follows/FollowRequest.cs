namespace LinkNest.Api.Controllers.V1.Follows
{
    public record FollowRequest(Guid followeeId, Guid followingId);
}
