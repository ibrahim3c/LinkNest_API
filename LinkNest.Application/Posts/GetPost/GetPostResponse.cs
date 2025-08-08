namespace LinkNest.Application.Posts.GetPost
{
    public sealed class GetPostResponse
    {
        public Guid Id { get; set; }    
        public string Content { get; private set; }
        public string CreatedAt { get; private set; }
        public string ImageUrl { get; private set; }
        public Guid UserProfileId { get; private set; }
    }
}
