using LinkNest.Application.Posts.DeleteInteractionToPost;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using Moq;

namespace LinkNest.Application.UnitTests.Posts
{
    public class DeleteInteractionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPostRepository> _postRepoMock;
        private readonly DeleteInteractionCommandHandler _handler;

        public DeleteInteractionCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _postRepoMock = new Mock<IPostRepository>();

            _unitOfWorkMock.SetupGet(u => u.PostRep).Returns(_postRepoMock.Object);

            _handler = new DeleteInteractionCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenInteractionNotFound()
        {
            // Arrange
            var command = new DeleteInteractionCommand(Guid.NewGuid());
            _postRepoMock.Setup(r => r.GetInteractionByIdAsync(command.interactionId))
                .ReturnsAsync((PostInteraction?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No Interaction Found", result.Errors);
            _postRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenPostNotFound()
        {
            // Arrange
            var interactionMock = new Mock<PostInteraction>();
            interactionMock.SetupGet(i => i.PostId).Returns(Guid.NewGuid());

            var command = new DeleteInteractionCommand(interactionMock.Object.Guid);

            _postRepoMock.Setup(r => r.GetInteractionByIdAsync(command.interactionId))
                .ReturnsAsync(interactionMock.Object);
            _postRepoMock.Setup(r => r.GetByIdAsync(interactionMock.Object.PostId))
                .ReturnsAsync((Post?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No Post Found", result.Errors);
        }

        [Fact]
        public async Task Handle_ShouldRemoveInteractionAndReturnSuccess()
        {
            // Arrange
            var interactionId = Guid.NewGuid();
            var postId = Guid.NewGuid();

            var interactionMock = new Mock<PostInteraction>();
            interactionMock.SetupGet(i => i.Guid).Returns(interactionId);
            interactionMock.SetupGet(i => i.PostId).Returns(postId);

            var postMock = new Mock<Post>(postId, new Content("content"), DateTime.UtcNow, new Url("https://img.png"), Guid.NewGuid());
            postMock.Setup(p => p.RemoveInteraction(interactionMock.Object));

            var command = new DeleteInteractionCommand(interactionId);

            _postRepoMock.Setup(r => r.GetInteractionByIdAsync(command.interactionId))
                .ReturnsAsync(interactionMock.Object);
            _postRepoMock.Setup(r => r.GetByIdAsync(interactionMock.Object.PostId))
                .ReturnsAsync(postMock.Object);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            postMock.Verify(p => p.RemoveInteraction(interactionMock.Object), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
