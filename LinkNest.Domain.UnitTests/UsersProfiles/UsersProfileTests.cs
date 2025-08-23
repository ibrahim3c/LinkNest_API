using ApartmentBooking.Domain.Users;
using FluentAssertions;
using LinkNest.Domain.UserProfiles;
using LinkNest.Domain.UserProfiles.DomainEvents;
using LinkNest.Domain.UserProfiles.DomainExceptions;


namespace LinkNest.Domain.UnitTests.UsersProfiles
{
    public class UsersProfileTests
    {
        [Fact]
        public void Create_Should_SetPropertyValues()
        {
            // Act
            var userProfile = UserProfile.Create(UserProfileData.FirstName, UserProfileData.LastName, UserProfileData.Email, new DateTime(1990, 1, 1), new CurrentCity("New York"), "appUserId123");
            // Assert
            userProfile.FirstName.Should().Be(UserProfileData.FirstName);
            userProfile.LastName.Should().Be(UserProfileData.LastName);
            userProfile.Email.Should().Be(UserProfileData.Email);
            userProfile.DateOfBirth.Should().Be(new DateTime(1990, 1, 1));
            userProfile.CurrentCity.Should().Be(new CurrentCity("New York"));
        }
        [Fact]
        public void Create_Should_ThrowException_When_DateOfBirthInFuture()
        {
            // Act
            Action act = () => UserProfile.Create(UserProfileData.FirstName, UserProfileData.LastName, UserProfileData.Email, DateTime.UtcNow.AddDays(1), new CurrentCity("New York"), "appUserId123");
            // Assert
            act.Should().Throw<UserProfileNotValidException>().WithMessage("Date of birth cannot be in the future.");
        }
        [Fact]
        public void Create_Should_ThrowException_When_NameIsNullOrWhiteSpace()
        {
            Action act = () => UserProfile.Create(
                 new FirstName(""),
                new LastName(""),
                UserProfileData.Email,
                new DateTime(1990, 1, 1),
                new CurrentCity("New York"),
                "appUserId123");

            act.Should()
               .Throw<UserProfileNotValidException>()
               .WithMessage("First name and last name cannot be empty.");
        }
        [Fact]
        public void Create_Should_ThrowException_When_EmailIsNullOrWhiteSpace()
        {
            Action act = () => UserProfile.Create(
                UserProfileData.FirstName,
                UserProfileData.LastName,
                null,
                new DateTime(1990, 1, 1),
                new CurrentCity("New York"),
                "appUserId123");
            act.Should()
               .Throw<UserProfileNotValidException>()
               .WithMessage("Email cannot be empty.");
        }
        [Fact]
        public void Create_Should_ThrowException_When_AppUserIdIsNullOrWhiteSpace()
        {
            Action act = () => UserProfile.Create(
                UserProfileData.FirstName,
                UserProfileData.LastName,
                UserProfileData.Email,
                new DateTime(1990, 1, 1),
                new CurrentCity("New York"),
                null);
            act.Should()
               .Throw<UserProfileNotValidException>()
               .WithMessage("App user ID cannot be empty.");
        }
        [Fact]
        public void Update_Should_UpdatePropertyValues()
        {
            // Arrange
            var userProfile = UserProfile.Create(UserProfileData.FirstName, UserProfileData.LastName, UserProfileData.Email, new DateTime(1990, 1, 1), new CurrentCity("New York"), "appUserId123");
            var newFirstName = new FirstName("Jane");
            var newLastName = new LastName("Smith");
            var newEmail = new UserProfileEmail("aho@gmail.com");
            var newDateOfBirth = new DateTime(1992, 2, 2);
            var newCurrentCity = new CurrentCity("Los Angeles");
            // Act
            userProfile.Update(newFirstName, newLastName, newEmail, newDateOfBirth, newCurrentCity);
            // Assert
            userProfile.FirstName.Should().Be(newFirstName);
            userProfile.LastName.Should().Be(newLastName);
            userProfile.Email.Should().Be(newEmail);
            userProfile.DateOfBirth.Should().Be(newDateOfBirth);
            userProfile.CurrentCity.Should().Be(newCurrentCity);
        }
        [Fact]
        public void Create_Should_RaiseUserProfileCreatedDomainEvent()
        {
            // Act
            var userProfile = UserProfile.Create(UserProfileData.FirstName, UserProfileData.LastName, UserProfileData.Email, new DateTime(1990, 1, 1), new CurrentCity("New York"), "appUserId123");
            //Assert
           var result= userProfile.GetDomainEvents().OfType<UserProfileCreatedDomainEvent>().SingleOrDefault();
            result.Should().NotBeNull();
            result.userProfileId.Should().Be(userProfile.Guid);
        }
    }
 }
