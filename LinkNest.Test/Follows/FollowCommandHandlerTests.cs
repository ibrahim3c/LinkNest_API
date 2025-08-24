using ApartmentBooking.Domain.Users;
using FluentAssertions;
using LinkNest.Application.Follows.FollowUserProfile;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Follows;
using LinkNest.Domain.UserProfiles;
using NSubstitute;

namespace LinkNest.Application.UnitTests.Follows
{
    public class FollowCommandHandlerTests
    {
        // milan course
        private static readonly FollowCommand followCommand = new(Guid.NewGuid(), Guid.NewGuid());

        private readonly IUnitOfWork unitOfWorkMock;
        private FollowCommandHandler handler;
        public FollowCommandHandlerTests()
        {
            // using NSubstitute
            unitOfWorkMock = Substitute.For<IUnitOfWork>();
            handler = new FollowCommandHandler(unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailture_WhenUserTriesToFollowHimself()
        {
            // Arrange
            var command = new FollowCommand(followCommand.followingId, followCommand.followingId);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEitherUserDoesNotExist()
        {
            // Arrange
            unitOfWorkMock.userProfileRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((UserProfile)null);
            // Act
            var result = await handler.Handle(followCommand, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenAlreadyFollowing()
        {
            var userProfile1 = UserProfile.Create(
                new FirstName("John"),
                new LastName("Doe"),
                new UserProfileEmail("test@gmail.com"), new DateTime(1990, 1, 1), new CurrentCity("New York"), "appUserId1");
            // Arrange
            unitOfWorkMock.userProfileRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(userProfile1);
            unitOfWorkMock.followRepo.IsFollowingAsync(Arg.Any<Guid>(), Arg.Any<Guid>()).Returns(true);

            // Act
            var result = await handler.Handle(followCommand, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenFollowIsSuccessful()
        {
            // Arrange 
            var userProfile1 = UserProfile.Create(
                new FirstName("John"),
                new LastName("Doe"),
                new UserProfileEmail("test@gmail.com"), new DateTime(1990, 1, 1), new CurrentCity("New York"), "appUserId1");
            var userProfile2 = UserProfile.Create(
                new FirstName("John2"),
                new LastName("Doe2"),
                new UserProfileEmail("test2@gmail.com"), new DateTime(1990, 1, 1), new CurrentCity("New York"), "appUserId2"); 
            unitOfWorkMock.userProfileRepo.GetByIdAsync(followCommand.followingId).Returns(userProfile1);
            unitOfWorkMock.userProfileRepo.GetByIdAsync(followCommand.followeeId).Returns(userProfile2);
            unitOfWorkMock.followRepo.IsFollowingAsync(followCommand.followingId, followCommand.followeeId).Returns(false);
            unitOfWorkMock.followRepo.AddAsync(Arg.Any<Follow>()).Returns(Task.CompletedTask);
            unitOfWorkMock.SaveChangesAsync().Returns(1);
            // Act
            var result = await handler.Handle(followCommand, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}
