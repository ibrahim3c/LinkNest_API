using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts.DomainExceptions;

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
            if (content == null) throw new PostCommentNotValidDomainException("Content cannot be null.");
            if (userProfileId == Guid.Empty) throw new PostCommentNotValidDomainException("UserProfileId cannot be empty.");
            if (postId == Guid.Empty) throw new PostCommentNotValidDomainException("PostId cannot be empty.");

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
            if (content == null) throw new ArgumentNullException("Content cannot be null.");

            this.Content = content;
        }
    }
}
