using LinkNest.Application.Posts.AddInteractionToPost;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using LinkNest.Domain.UserProfiles;
using Moq;

namespace LinkNest.Application.UnitTests.Posts
{
    public class AddInteractionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPostRepository> _postRepoMock;
        private readonly Mock<IUserProfileRepository> _userProfileRepoMock;
        private readonly AddInteractionCommandHandler _handler;

        public AddInteractionCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _postRepoMock = new Mock<IPostRepository>();
            _userProfileRepoMock = new Mock<IUserProfileRepository>();

            _unitOfWorkMock.SetupGet(u => u.PostRep).Returns(_postRepoMock.Object);
            _unitOfWorkMock.SetupGet(u => u.userProfileRepo).Returns(_userProfileRepoMock.Object);

            _handler = new AddInteractionCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenPostNotFound()
        {
            // Arrange
            var command = new AddInteractionCommand(Guid.NewGuid(), Guid.NewGuid(), InteractionTypes.Like);
            _postRepoMock.Setup(r => r.GetByIdAsync(command.postId))
                .ReturnsAsync((Post?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No Post Found", result.Errors);
            _userProfileRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var command = new AddInteractionCommand(Guid.NewGuid(), Guid.NewGuid(), InteractionTypes.Like);

            var postMock = new Mock<Post>(Guid.NewGuid(), new Content("content"), DateTime.UtcNow, new Url("https://img.png"), Guid.NewGuid());
            _postRepoMock.Setup(r => r.GetByIdAsync(command.postId)).ReturnsAsync(postMock.Object);

            _userProfileRepoMock.Setup(r => r.GetByIdAsync(command.userProfileId))
                .ReturnsAsync((UserProfile?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No User Found", result.Errors);
            postMock.Verify(p => p.AddInteraction(It.IsAny<PostInteraction>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldAddInteractionAndReturnSuccess()
        {
            // Arrange
            var command = new AddInteractionCommand(Guid.NewGuid(), Guid.NewGuid(), InteractionTypes.Like);

            var postMock = new Mock<Post>(Guid.NewGuid(), new Content("content"), DateTime.UtcNow, new Url("https://img.png"), Guid.NewGuid());
            var userMock = new Mock<UserProfile>(Guid.NewGuid(), "first", "last", "email@test.com", DateTime.UtcNow, DateTime.UtcNow, null);

            _postRepoMock.Setup(r => r.GetByIdAsync(command.postId)).ReturnsAsync(postMock.Object);
            _userProfileRepoMock.Setup(r => r.GetByIdAsync(command.userProfileId)).ReturnsAsync(userMock.Object);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
            postMock.Verify(p => p.AddInteraction(It.IsAny<PostInteraction>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
