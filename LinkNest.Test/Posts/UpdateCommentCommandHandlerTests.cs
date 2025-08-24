using LinkNest.Application.Posts.UpdateCommentToPost;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using Moq;

namespace LinkNest.Application.UnitTests.Posts
{
    public class UpdateCommentCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPostRepository> _postRepoMock;
        private readonly UpdateCommentCommandHandler _handler;

        public UpdateCommentCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _postRepoMock = new Mock<IPostRepository>();

            _unitOfWorkMock.SetupGet(u => u.PostRep).Returns(_postRepoMock.Object);

            _handler = new UpdateCommentCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCommentNotFound()
        {
            // Arrange
            var command = new UpdateCommentCommand(Guid.NewGuid(), "Updated content");
            _postRepoMock.Setup(r => r.GetCommentByIdAsync(command.commandId))
                .ReturnsAsync((PostComment?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No Command Found", result.Errors);
        }

        [Fact]
        public async Task Handle_ShouldUpdateCommentAndReturnSuccess()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var command = new UpdateCommentCommand(commentId, "Updated content");

            var commentMock = new Mock<PostComment>();
            commentMock.SetupGet(c => c.Guid).Returns(commentId);

            _postRepoMock.Setup(r => r.GetCommentByIdAsync(command.commandId))
                .ReturnsAsync(commentMock.Object);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            commentMock.Verify(c => c.UpdateConent(It.Is<Content>(ct => ct.content == "Updated content")), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
