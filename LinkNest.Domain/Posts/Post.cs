using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts.DomainEvents;

namespace LinkNest.Domain.Posts
{
    public class Post : Entity
    {
        public Post(Guid guid, Content content, DateTime createdAt, Url imageUrl, Guid userProfileId) : base(guid)
        {
            Content = content;
            CreatedAt = createdAt;
            ImageUrl = imageUrl;
            UserProfileId = userProfileId;
        }
        // for EF Core
        private Post() { }
        public Content Content { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public Url ImageUrl { get; private set; }
        public Guid UserProfileId { get; private set; }

        // Navigation properties
        public ICollection<PostComment> Comments { get; private set; } = new List<PostComment>();
        public ICollection<PostInteraction> Interactions { get; private set; } = new List<PostInteraction>();

        // Factory method to create a new Post instance
        public static Post Create(Content content,  Url imageUrl, Guid userProfileId)
        {
            // TODO: Validate parameters as needed and raise domain events or perform additional logic if needed
            var post = new Post(Guid.NewGuid(), content, DateTime.UtcNow, imageUrl, userProfileId);

            post.RaiseDomainEvent(new PostCreatedEvent(post.Guid, content,DateTime.UtcNow, imageUrl, userProfileId));
            return post;

        }
        // update post content
        public void UpdateContent(Content content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            Content = content;
        }

        // Post comment methods
        public void AddComment(PostComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            Comments.Add(comment);
            RaiseDomainEvent(new PostCommentAddedEvent(comment.Guid, comment.PostId, comment.UserProfileId, comment.Content, comment.CreatedAt));
        }
        public void RemoveComment(PostComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            if (!Comments.Remove(comment))
            {
                throw new InvalidOperationException("Comment not found in the post.");
            }
        }

        // Post interaction methods
        public void AddInteraction(PostInteraction interaction)
        {
            if (interaction == null) throw new ArgumentNullException(nameof(interaction));
            Interactions.Add(interaction);
            RaiseDomainEvent(new PostInteractionAddedEvent(interaction.Guid, interaction.PostId, interaction.UserProfileId, interaction.CreatedAt));
        }
        public void RemoveInteraction(PostInteraction interaction)
        {
            if (interaction == null) throw new ArgumentNullException(nameof(interaction));
            if (!Interactions.Remove(interaction))
            {
                throw new InvalidOperationException("Interaction not found in the post.");
            }
        }

        public void ClearComments()
        {
            Comments.Clear();
        }
        public void ClearInteractions()
        {
            Interactions.Clear();
        }

        public void RemoveAllRelatedData()
        {
            Comments.Clear();
            Interactions.Clear();
        }
    }
}
