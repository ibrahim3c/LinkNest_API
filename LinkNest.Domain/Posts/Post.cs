using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts.DomainEvents;
using LinkNest.Domain.Posts.DomainExceptions;

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
            if (content == null) throw new PostNotValidDomainException("Content cannot be null.");
            if (userProfileId == Guid.Empty) throw new PostNotValidDomainException("UserProfileId cannot be empty.");
            if (imageUrl == null) throw new PostNotValidDomainException("ImageUrl cannot be null.");

            var post = new Post(Guid.NewGuid(), content, DateTime.UtcNow, imageUrl, userProfileId);

            post.RaiseDomainEvent(new PostCreatedDomainEvent(post.Guid, content,DateTime.UtcNow, imageUrl, userProfileId));
            return post;

        }
        // update post content
        public void UpdateContent(Content content)
        {
            if (content == null) throw new PostNotValidDomainException("Content cannot be null.");
            Content = content;
        }

        // Post comment methods
        public void AddComment(PostComment comment)
        {
            if (comment == null) throw new PostNotValidDomainException("Content cannot be null.");
            Comments.Add(comment);
            RaiseDomainEvent(new PostCommentAddedDomainEvent(comment.Guid, comment.PostId, comment.UserProfileId, comment.Content, comment.CreatedAt));
        }
        public void RemoveComment(PostComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            if (!Comments.Remove(comment))
            {
                throw new PostNotValidDomainException("Comment not found in the post.");
            }
        }

        // Post interaction methods
        public void AddInteraction(PostInteraction interaction)
        {
            if (interaction == null) throw new PostNotValidDomainException("Interaction cannot be null.");
            Interactions.Add(interaction);
            RaiseDomainEvent(new PostInteractionAddedDomainEvent(interaction.Guid, interaction.PostId, interaction.UserProfileId, interaction.CreatedAt));
        }
        public void RemoveInteraction(PostInteraction interaction)
        {
            if (interaction == null) throw new PostNotValidDomainException(nameof(interaction));
            if (!Interactions.Remove(interaction))
            {
                throw new PostNotValidDomainException("Interaction not found in the post.");
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
