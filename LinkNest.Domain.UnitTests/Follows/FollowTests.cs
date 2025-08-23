using FluentAssertions;
using LinkNest.Domain.Follows;
using LinkNest.Domain.Follows.DomainEvents;
using LinkNest.Domain.Follows.DomainExceptions;

namespace LinkNest.Domain.UnitTests.Follows
{
    public class FollowTests
    {
        [Fact]
        public void Constructor_Should_SetProperties_When_ValidInput()
        {
            // Arrange
            var followerId = Guid.NewGuid();
            var followeeId = Guid.NewGuid();

            // Act
            var follow = new Follow(Guid.NewGuid(), followerId, followeeId);

            // Assert
            follow.FollowerId.Should().Be(followerId);
            follow.FolloweeId.Should().Be(followeeId);
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_FollowerIdIsEmpty()
        {
            // Act
            Action act = () => new Follow(Guid.NewGuid(), Guid.Empty, Guid.NewGuid());

            // Assert
            act.Should()
               .Throw<FollowRequestNotValidDomainException>()
               .WithMessage("Follower ID cannot be empty.");
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_FolloweeIdIsEmpty()
        {
            // Act
            Action act = () => new Follow(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty);

            // Assert
            act.Should()
               .Throw<FollowRequestNotValidDomainException>()
               .WithMessage("Followee ID cannot be empty.");
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_UserFollowsThemselves()
        {
            // Arrange
            var sameId = Guid.NewGuid();

            // Act
            Action act = () => new Follow(Guid.NewGuid(), sameId, sameId);

            // Assert
            act.Should()
               .Throw<FollowRequestNotValidDomainException>()
               .WithMessage("User cannot follow themselves.");
        }

        [Fact]
        public void Create_Should_SetPropertiesAndRaiseDomainEvent()
        {
            // Arrange
            var followerId = Guid.NewGuid();
            var followeeId = Guid.NewGuid();

            // Act
            var follow = Follow.Create(followerId, followeeId);

            // Assert
            follow.FollowerId.Should().Be(followerId);
            follow.FolloweeId.Should().Be(followeeId);

            var domainEvent = follow.GetDomainEvents()
                                    .OfType<FollowCreatedDomainEvent>()
                                    .SingleOrDefault();

            domainEvent.Should().NotBeNull();
            domainEvent.FolloweeId.Should().Be(followeeId);
            domainEvent.FollowerId.Should().Be(followerId);
        }
    }
}
