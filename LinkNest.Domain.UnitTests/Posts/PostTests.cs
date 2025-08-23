using FluentAssertions;
using LinkNest.Domain.Posts;
using LinkNest.Domain.Posts.DomainEvents;
using LinkNest.Domain.Posts.DomainExceptions;

namespace LinkNest.Domain.UnitTests.Posts
{
    public class PostTests
    {
        [Fact]
        public void Create_Should_SetPropertiesCorrectly()
        {
            // Arrange
            var content = new Content("This is a test post");
            var imageUrl = new Url("http://test.com/image.jpg");
            var userProfileId = Guid.NewGuid();

            // Act
            var post = Post.Create(content, imageUrl, userProfileId);

            // Assert
            post.Content.Should().Be(content);
            post.ImageUrl.Should().Be(imageUrl);
            post.UserProfileId.Should().Be(userProfileId);
            post.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void Create_Should_ThrowException_When_ContentIsNull()
        {
            // Act
            Action act = () => Post.Create(null, new Url("http://test.com"), Guid.NewGuid());

            // Assert
            act.Should().Throw<PostNotValidDomainException>()
                .WithMessage("Content cannot be null.");
        }

        [Fact]
        public void Create_Should_ThrowException_When_UserProfileIdIsEmpty()
        {
            // Act
            Action act = () => Post.Create(new Content("test"), new Url("http://test.com"), Guid.Empty);

            // Assert
            act.Should().Throw<PostNotValidDomainException>()
                .WithMessage("UserProfileId cannot be empty.");
        }

        [Fact]
        public void Create_Should_ThrowException_When_ImageUrlIsNull()
        {
            // Act
            Action act = () => Post.Create(new Content("test"), null, Guid.NewGuid());

            // Assert
            act.Should().Throw<PostNotValidDomainException>()
                .WithMessage("ImageUrl cannot be null.");
        }

        [Fact]
        public void Create_Should_Raise_PostCreatedDomainEvent()
        {
            // Arrange
            var content = new Content("Hello World");
            var imageUrl = new Url("http://test.com/image.png");
            var userProfileId = Guid.NewGuid();

            // Act
            var post = Post.Create(content, imageUrl, userProfileId);

            // Assert
            var @event = post.GetDomainEvents().OfType<PostCreatedDomainEvent>().SingleOrDefault();
            @event.Should().NotBeNull();
            @event!.postId.Should().Be(post.Guid);
            @event.Content.Should().Be(content);
            @event.imageUrl.Should().Be(imageUrl);
            @event.userProfileId.Should().Be(userProfileId);
        }

        [Fact]
        public void UpdateContent_Should_Update_PostContent()
        {
            // Arrange
            var post = Post.Create(new Content("Old"), new Url("http://test.com"), Guid.NewGuid());
            var newContent = new Content("Updated Content");

            // Act
            post.UpdateContent(newContent);

            // Assert
            post.Content.Should().Be(newContent);
        }

        [Fact]
        public void UpdateContent_Should_ThrowException_When_ContentIsNull()
        {
            var post = Post.Create(new Content("test"), new Url("http://test.com"), Guid.NewGuid());

            Action act = () => post.UpdateContent(null);

            act.Should().Throw<PostNotValidDomainException>()
                .WithMessage("Content cannot be null.");
        }

        [Fact]
        public void AddComment_Should_AddComment_AndRaiseDomainEvent()
        {
            // Arrange
            var post = Post.Create(new Content("test"), new Url("http://test.com"), Guid.NewGuid());
            var comment = new PostComment(post.Guid, new Content("Nice post"), DateTime.UtcNow, Guid.NewGuid());

            // Act
            post.AddComment(comment);

            // Assert
            post.Comments.Should().Contain(comment);

            var @event = post.GetDomainEvents().OfType<PostCommentAddedDomainEvent>().SingleOrDefault();
            @event.Should().NotBeNull();
            @event!.postId.Should().Be(comment.PostId);
            @event.userProfileId.Should().Be(comment.UserProfileId);
        }

        [Fact]
        public void RemoveComment_Should_Remove_Comment()
        {
            var post = Post.Create(new Content("test"), new Url("http://test.com"), Guid.NewGuid());
            var comment = new PostComment(post.Guid, new Content("Nice post") , DateTime.UtcNow, Guid.NewGuid());
            post.AddComment(comment);

            post.RemoveComment(comment);

            post.Comments.Should().NotContain(comment);
        }

        [Fact]
        public void RemoveComment_Should_ThrowException_When_CommentNotFound()
        {
            var post = Post.Create(new Content("test"), new Url("http://test.com"), Guid.NewGuid());
            var comment = new PostComment(post.Guid, new Content("Nice post"), DateTime.UtcNow, Guid.NewGuid());

            Action act = () => post.RemoveComment(comment);

            act.Should().Throw<PostNotValidDomainException>()
                .WithMessage("Comment not found in the post.");
        }

        [Fact]
        public void AddInteraction_Should_AddInteraction_AndRaiseDomainEvent()
        {
            var post = Post.Create(new Content("test"), new Url("http://test.com"), Guid.NewGuid());
            var interaction = new PostInteraction(Guid.NewGuid(),post.Guid, Guid.NewGuid(), DateTime.UtcNow,InteractionTypes.Like);
            var comment = new PostComment(post.Guid, new Content("Nice post"), DateTime.UtcNow, Guid.NewGuid());


            post.AddInteraction(interaction);

            post.Interactions.Should().Contain(interaction);

            var @event = post.GetDomainEvents().OfType<PostInteractionAddedDomainEvent>().SingleOrDefault();
            @event.Should().NotBeNull();
            @event!.postId.Should().Be(interaction.PostId);
            @event.userProfileId.Should().Be(interaction.UserProfileId);
        }

        [Fact]
        public void RemoveInteraction_Should_Remove_Interaction()
        {
            var post = Post.Create(new Content("test"), new Url("http://test.com"), Guid.NewGuid());
            var interaction = new PostInteraction(Guid.NewGuid(), post.Guid, Guid.NewGuid(), DateTime.UtcNow, InteractionTypes.Like);
            post.AddInteraction(interaction);

            post.RemoveInteraction(interaction);

            post.Interactions.Should().NotContain(interaction);
        }

        [Fact]
        public void RemoveInteraction_Should_ThrowException_When_NotFound()
        {
            var post = Post.Create(new Content("test"), new Url("http://test.com"), Guid.NewGuid());
            var interaction = new PostInteraction(Guid.NewGuid(), post.Guid, Guid.NewGuid(), DateTime.UtcNow, InteractionTypes.Like);

            Action act = () => post.RemoveInteraction(interaction);

            act.Should().Throw<PostNotValidDomainException>()
                .WithMessage("Interaction not found in the post.");
        }

        [Fact]
        public void ClearComments_Should_Remove_AllComments()
        {
            var post = Post.Create(new Content("test"), new Url("http://test.com"), Guid.NewGuid());
            var interaction = new PostInteraction(Guid.NewGuid(), post.Guid, Guid.NewGuid(), DateTime.UtcNow, InteractionTypes.Like);

            post.ClearComments();

            post.Comments.Should().BeEmpty();
        }

        [Fact]
        public void ClearInteractions_Should_Remove_AllInteractions()
        {
            var post = Post.Create(new Content("test"), new Url("http://test.com"), Guid.NewGuid());
            var interaction = new PostInteraction(Guid.NewGuid(), post.Guid, Guid.NewGuid(), DateTime.UtcNow, InteractionTypes.Like);

            post.ClearInteractions();

            post.Interactions.Should().BeEmpty();
        }

        [Fact]
        public void RemoveAllRelatedData_Should_Clear_Comments_And_Interactions()
        {
            var post = Post.Create(new Content("test"), new Url("http://test.com"), Guid.NewGuid());
            var comment = new PostComment(post.Guid, new Content("Nice post"), DateTime.UtcNow, Guid.NewGuid());
            var interaction = new PostInteraction(Guid.NewGuid(), post.Guid, Guid.NewGuid(), DateTime.UtcNow, InteractionTypes.Like);

            post.RemoveAllRelatedData();

            post.Comments.Should().BeEmpty();
            post.Interactions.Should().BeEmpty();
        }
    }
}

