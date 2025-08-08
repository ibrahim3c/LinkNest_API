using LinkNest.Application.Posts.GetPost;

namespace LinkNest.Application.Posts.GetUserPosts
{
    public sealed class GetUserPostsResponse
    {
        public Guid userProfileId {  get; init; }
        public List<GetPostResponse> posts { get; init; }

    }
}
