namespace LinkNest.Application.Posts.GetPostComments
{
    public class CommentInfo
    {
        public string Content { get; init; }
        public DateTime CreatedAt { get; init; }
        public Guid UserProfileId { get; init; }
    }
    public class GetPostCommentsResponse
    {
        public Guid PostId { get; init; }
        public List<CommentInfo> PostComments { get; init; }


    }
}
