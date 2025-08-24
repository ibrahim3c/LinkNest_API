using ApartmentBooking.Domain.Users;
using FluentAssertions;
using LinkNest.Application.Posts.AddCommentToPost;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using LinkNest.Domain.UserProfiles;
using Moq;
using NSubstitute;

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
            var post=new Post(Guid.NewGuid(), new Content("Original Post"), DateTime.UtcNow, new Url("http://example.com/image.jpg"), Guid.NewGuid());
            var userProfile = UserProfile.Create(
                new FirstName("John"),
                new LastName("Doe"),
                new UserProfileEmail("test@gmail.com"), new DateTime(1990, 1, 1), new CurrentCity("New York"), "appUserId1");


                _postRepoMock.Setup(r => r.GetByIdAsync(command.PostId)).ReturnsAsync(post);
            _userProfileRepoMock.Setup(r => r.GetByIdAsync(command.UserProfileId)).ReturnsAsync(userProfile);


            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }

}