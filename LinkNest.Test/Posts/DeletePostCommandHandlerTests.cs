using LinkNest.Application.Posts.DeletePost;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using Moq;

namespace LinkNest.Application.UnitTests.Posts
{
    public class DeletePostCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPostRepository> _postRepoMock;
        private readonly DeletePostCommandHandler _handler;

        public DeletePostCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _postRepoMock = new Mock<IPostRepository>();

            _unitOfWorkMock.SetupGet(u => u.PostRep).Returns(_postRepoMock.Object);

            _handler = new DeletePostCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenPostNotFound()
        {
            // Arrange
            var command = new DeletePostCommand(Guid.NewGuid());
            _postRepoMock.Setup(r => r.GetByIdAsync(command.postId))
                .ReturnsAsync((Post?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No Post Found", result.Errors);
            _postRepoMock.Verify(r => r.Delete(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDeletePostAndReturnSuccess()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var postMock = new Mock<Post>(
                postId,
                new Content("test content"),
                DateTime.UtcNow,
                new Url("https://img.png"),
                Guid.NewGuid()
            );

            var command = new DeletePostCommand(postId);

            _postRepoMock.Setup(r => r.GetByIdAsync(command.postId))
                .ReturnsAsync(postMock.Object);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _postRepoMock.Verify(r => r.Delete(postMock.Object), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
