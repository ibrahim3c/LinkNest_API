using LinkNest.Application.Posts.AddCommentToPost;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using LinkNest.Domain.UserProfiles;
using Moq;

namespace LinkNest.Application.UnitTests.Posts
{
    public class AddCommentCommandHandlerTests
    {

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPostRepository> _postRepoMock;
        private readonly Mock<IUserProfileRepository> _userProfileRepoMock;
        private readonly AddCommentCommandHandler _handler;

        public AddCommentCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _postRepoMock = new Mock<IPostRepository>();
            _userProfileRepoMock = new Mock<IUserProfileRepository>();

            _unitOfWorkMock.SetupGet(u => u.PostRep).Returns(_postRepoMock.Object);
            _unitOfWorkMock.SetupGet(u => u.userProfileRepo).Returns(_userProfileRepoMock.Object);

            _handler = new AddCommentCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenPostNotFound()
        {
            // Arrange
            var command = new AddCommentCommand("Test content", Guid.NewGuid(), Guid.NewGuid());
            _postRepoMock.Setup(r => r.GetByIdAsync(command.PostId)).ReturnsAsync((Post)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No Post Found", result.Errors);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var command = new AddCommentCommand("Test content", Guid.NewGuid(), Guid.NewGuid());
            var post = new Mock<Post>(Guid.NewGuid(), null, DateTime.UtcNow, null, Guid.NewGuid()).Object;
            _postRepoMock.Setup(r => r.GetByIdAsync(command.PostId)).ReturnsAsync(post);
            _userProfileRepoMock.Setup(r => r.GetByIdAsync(command.UserProfileId)).ReturnsAsync((UserProfile)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No User Found", result.Errors);
        }

        [Fact]
        public async Task Handle_ShouldAddCommentAndReturnSuccess()
        {
            // Arrange
            var command = new AddCommentCommand("Test content", Guid.NewGuid(), Guid.NewGuid());
            var postMock = new Mock<Post>(Guid.NewGuid(), null, DateTime.UtcNow, null, Guid.NewGuid());
            var userMock = new Mock<UserProfile>(Guid.NewGuid(), null, null, null, DateTime.UtcNow, DateTime.UtcNow, null);
            var commentGuid = Guid.NewGuid();

            var commentMock = new Mock<PostComment>();
            commentMock.SetupGet(c => c.Guid).Returns(commentGuid);

            _postRepoMock.Setup(r => r.GetByIdAsync(command.PostId)).ReturnsAsync(postMock.Object);
            _userProfileRepoMock.Setup(r => r.GetByIdAsync(command.UserProfileId)).ReturnsAsync(userMock.Object);

            // Use reflection to call the static Create method
            typeof(PostComment)
                .GetMethod("Create")
                ?.Invoke(null, new object[] { new Content(command.Content), command.PostId, command.UserProfileId });

            postMock.Setup(p => p.AddComment(It.IsAny<PostComment>()));

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            // Can't check the exact Guid unless Create is mocked, so just check it's not empty
            Assert.NotEqual(Guid.Empty, result.Value);
        }
    }

}