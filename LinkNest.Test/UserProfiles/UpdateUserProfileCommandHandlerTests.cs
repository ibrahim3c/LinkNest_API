using ApartmentBooking.Domain.Users;
using FluentAssertions;
using LinkNest.Application.UserProfiles.UpdateUserProfile;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Identity;
using LinkNest.Domain.UserProfiles;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace LinkNest.Application.UnitTests.UserProfiles
{
    public class UpdateUserProfileCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly UpdateUserProfileCommandHandler handler;

        public UpdateUserProfileCommandHandlerTests()
        {
            _unitOfWorkMock = new();
            _userManagerMock =UserManagerMockHelper.CreateMockUserManager<AppUser>();
            handler = new(_unitOfWorkMock.Object, _userManagerMock.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUserProfileNotFound()
        {
            // Arrange
            var command = new UpdateUserProfileCommand(Guid.NewGuid(),"john","wika","testo@gmail.com",DateTime.Now, "Tokyo", "045888937384");

            _unitOfWorkMock.Setup(u=>u.userProfileRepo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((UserProfile)null); // Simulate user profile not found

            // Act
            var result=await handler.Handle(command, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Handle_Should_ReturnFailure_WhenEmailAlreadyExist()
        {
            // Arrange
            var command = new UpdateUserProfileCommand(Guid.NewGuid(), "john", "wika", "testo@gmail.com", DateTime.Now, "Tokyo", "045888937384");
            _unitOfWorkMock.Setup(u => u.userProfileRepo.IsEmailExist(It.IsAny<string>()))
                .ReturnsAsync(true); // Simulate user profile not found

            // Act
            var result = handler.Handle(command, CancellationToken.None).Result;

            // Assert
            result.IsSuccess.Should().BeFalse();
        }


        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUserIdentityUserNotFound()
        {
            // Arrange
            var userProfile = new UserProfile(
                                   Guid.NewGuid(),
                                   new FirstName("Ali"),
                                   new LastName("Hassan"),
                                   new UserProfileEmail("ali@example.com"),
                                   DateTime.Now.AddYears(-3),
                                   DateTime.UtcNow,
                                   new CurrentCity("Giza"),
                                   Guid.NewGuid().ToString());
            var command = new UpdateUserProfileCommand(userProfile.Guid, "john", "wika", "testo@gmail.com", DateTime.Now.AddYears(-3), "Tokyo", "045888937384");

            _unitOfWorkMock.Setup(u => u.userProfileRepo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userProfile);
            _unitOfWorkMock.Setup(u => u.userProfileRepo.IsEmailExist(It.IsAny<string>()))
                .ReturnsAsync(false);
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null); // Simulate user identity not found

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public async Task Handle_ShouldUpdateUserProfile_WhenValid()
        {
            // Arrange
            var appUser = new AppUser { Id = Guid.NewGuid().ToString(), UserName = "AliHassan", PhoneNumber = "0000" };
            var userProfile = new UserProfile(
                                   Guid.NewGuid(),
                                   new FirstName("Ali"),
                                   new LastName("Hassan"),
                                   new UserProfileEmail("ali@example.com"),
                                   DateTime.Now.AddYears(-3),
                                   DateTime.UtcNow,
                                   new CurrentCity("Giza"),
                                   appUser.Id);
            var command = new UpdateUserProfileCommand(userProfile.Guid, "john", "wika", "testo@gmail.com", DateTime.Now.AddYears(-3), "Tokyo", "045888937384");


            _unitOfWorkMock.Setup(u => u.userProfileRepo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userProfile);
            _unitOfWorkMock.Setup(u => u.userProfileRepo.IsEmailExist(It.IsAny<string>()))
                .ReturnsAsync(false);

            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(appUser); 

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeTrue();

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

        }
    }
}
