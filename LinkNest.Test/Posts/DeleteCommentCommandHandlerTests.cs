using LinkNest.Application.Posts.DeleteCommentToPost;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using Moq;

namespace LinkNest.Application.UnitTests.Posts
{
    public class DeleteCommentCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPostRepository> _postRepoMock;
        private readonly DeleteCommentCommandHandler _handler;

        public DeleteCommentCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _postRepoMock = new Mock<IPostRepository>();

            _unitOfWorkMock.SetupGet(u => u.PostRep).Returns(_postRepoMock.Object);

            _handler = new DeleteCommentCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCommentNotFound()
        {
            // Arrange
            var command = new DeleteCommentCommand(Guid.NewGuid());
            _postRepoMock.Setup(r => r.GetCommentByIdAsync(command.CommentId))
                .ReturnsAsync((PostComment?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No Comment Found", result.Errors);
            _postRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenPostNotFound()
        {
            // Arrange
            var comment = new Mock<PostComment>();
            comment.SetupGet(c => c.PostId).Returns(Guid.NewGuid());

            var command = new DeleteCommentCommand(comment.Object.Guid);

            _postRepoMock.Setup(r => r.GetCommentByIdAsync(command.CommentId))
                .ReturnsAsync(comment.Object);
            _postRepoMock.Setup(r => r.GetByIdAsync(comment.Object.PostId))
                .ReturnsAsync((Post?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("No Post Found", result.Errors);
        }

        [Fact]
        public async Task Handle_ShouldRemoveCommentAndReturnSuccess()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var postId = Guid.NewGuid();

            var commentMock = new Mock<PostComment>();
            commentMock.SetupGet(c => c.Guid).Returns(commentId);
            commentMock.SetupGet(c => c.PostId).Returns(postId);

            var postMock = new Mock<Post>(postId, new Content("content"), DateTime.UtcNow, new Url("https://img.png"), Guid.NewGuid());
            postMock.Setup(p => p.RemoveComment(commentMock.Object));

            var command = new DeleteCommentCommand(commentId);

            _postRepoMock.Setup(r => r.GetCommentByIdAsync(command.CommentId))
                .ReturnsAsync(commentMock.Object);
            _postRepoMock.Setup(r => r.GetByIdAsync(commentMock.Object.PostId))
                .ReturnsAsync(postMock.Object);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            postMock.Verify(p => p.RemoveComment(commentMock.Object), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
