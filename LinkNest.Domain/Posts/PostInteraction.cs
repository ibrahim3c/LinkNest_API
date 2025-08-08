using LinkNest.Domain.Abstraction;

namespace LinkNest.Domain.Posts
{
    public class PostInteraction : Entity
    {
        public PostInteraction(Guid guid, Guid postId,Guid userProfileId,DateTime createdAt,InteractionTypes interactionType) : base(guid)
        {
            PostId = postId;
            UserProfileId = userProfileId;
            CreatedAt = createdAt;
            InteractionType = interactionType;
        }
        private PostInteraction() { }
        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public InteractionTypes InteractionType { get; private set; }
        // Navigation properties
        public Post Post { get; private set; } = null!; // Ensures Post is not null after initialization

        public static PostInteraction Create(Guid postId, Guid userProfileId, InteractionTypes interactionType)
        {
            return new PostInteraction
            {
                CreatedAt = DateTime.UtcNow,
                PostId = postId,
                InteractionType = interactionType,
                UserProfileId = userProfileId,
                Guid= Guid.NewGuid()
            };
        }
    }
}
