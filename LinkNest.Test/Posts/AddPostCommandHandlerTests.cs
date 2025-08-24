using LinkNest.Application.Posts.AddPost;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using LinkNest.Domain.UserProfiles;
using Moq;

namespace LinkNest.Application.UnitTests.Posts
{
    public class AddPostCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserProfileRepository> _userProfileRepoMock;
        private readonly Mock<IPostRepository> _postRepoMock;
        private readonly AddPostCommandHandler _handler;

        public AddPostCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userProfileRepoMock = new Mock<IUserProfileRepository>();
            _postRepoMock = new Mock<IPostRepository>();

            _unitOfWorkMock.SetupGet(u => u.userProfileRepo).Returns(_userProfileRepoMock.Object);
            _unitOfWorkMock.SetupGet(u => u.PostRep).Returns(_postRepoMock.Object);

            _handler = new AddPostCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var command = new AddPostCommand("Test content", "https://image.com/img.png", Guid.NewGuid());
            _userProfileRepoMock.Setup(r => r.GetByIdAsync(command.UserProfileId)).ReturnsAsync((UserProfile?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No User Profile Found", result.Errors);
            _postRepoMock.Verify(r => r.AddAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldAddPostAndReturnSuccess()
        {
            // Arrange
            var command = new AddPostCommand("Test content", "https://image.com/img.png", Guid.NewGuid());

            var userProfileMock = new Mock<UserProfile>(
                Guid.NewGuid(),
                "first",
                "last",
                "email@test.com",
                DateTime.UtcNow,
                DateTime.UtcNow,
                null
            );

            _userProfileRepoMock.Setup(r => r.GetByIdAsync(command.UserProfileId)).ReturnsAsync(userProfileMock.Object);

            _postRepoMock.Setup(r => r.AddAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
            _postRepoMock.Verify(r => r.AddAsync(It.IsAny<Post>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
