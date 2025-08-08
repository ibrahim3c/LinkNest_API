using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Posts.GetUserPosts
{
    public sealed record GetUserPostsQuery(Guid UserProfileId):IQuery<GetUserPostsResponse>;
}
