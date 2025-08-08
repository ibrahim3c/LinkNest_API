using LinkNest.Domain.Abstraction;

namespace LinkNest.Domain.Posts
{
    public class PostComment : Entity
    {
        public PostComment(Guid guid, Content content, DateTime createdAt, Guid userProfileId) : base(guid)
        {
            Content = content;
            CreatedAt = createdAt;
            UserProfileId = userProfileId;
        }
        private PostComment() { }
        public Content Content { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }

        // Navigation properties
        public Post Post { get; private set; } = null!; // Ensures Post is not null after initialization


        public static PostComment Create(Content content,Guid postId, Guid userProfileId)
        {
            return new PostComment
            {
                Content = content,
                CreatedAt = DateTime.UtcNow,
                UserProfileId = userProfileId,
                PostId=postId,
                Guid= Guid.NewGuid()
            };
        }

        public void UpdateConent(Content content)
        {
            this.Content = content;
        }
    }
}
