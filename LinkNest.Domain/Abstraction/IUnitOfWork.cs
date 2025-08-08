using LinkNest.Domain.Follows;
using LinkNest.Domain.Posts;
using LinkNest.Domain.UserProfiles;

namespace LinkNest.Domain.Abstraction
{
    public interface IUnitOfWork
    {
        IPostRepository PostRep { get; }
        IUserProfileRepository userProfileRepo { get; }
        IFollowRepository followRepo { get; }

        Task<int> SaveChangesAsync();
    }
}
