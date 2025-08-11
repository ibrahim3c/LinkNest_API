using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Follows.DomainEvents;
using LinkNest.Domain.Follows.DomainExceptions;
using LinkNest.Domain.UserProfiles;

namespace LinkNest.Domain.Follows
{
    public class Follow : Entity
    {
        public Guid FollowerId { get; private set; } // who follows
        public Guid FolloweeId { get; private set; } // who is being followed

        private Follow()
        {
            
        }
        public Follow(Guid id, Guid followerId, Guid followeeId):base(id) 
        {
            if (followerId == Guid.Empty)
                throw new FollowRequestNotValidDomainException("Follower ID cannot be empty.");
            if (followeeId == Guid.Empty)
                throw new FollowRequestNotValidDomainException("Followee ID cannot be empty.");
            if (followerId == followeeId)
                throw new FollowRequestNotValidDomainException("User cannot follow themselves.");

            this.FollowerId = followerId;
            this.FolloweeId = followeeId;
        }

        public static Follow Create(Guid followerId, Guid followeeId)
        {
            var follow= new Follow
            {
                FolloweeId = followeeId,
                FollowerId = followerId,
                Guid= Guid.NewGuid()
            };
            follow.RaiseDomainEvent(new FollowCreatedDomainEvent(followeeId, followerId));
            return follow;

        }
        public UserProfile Follower {  get; private set; }
        public UserProfile Followee {  get; private set; }

    }
}
