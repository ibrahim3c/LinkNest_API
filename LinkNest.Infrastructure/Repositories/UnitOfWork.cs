using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Follows;
using LinkNest.Domain.Posts;
using LinkNest.Domain.UserProfiles;
using LinkNest.Infrastructure.Data;

namespace LinkNest.Infrastructure.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            PostRep = new PostRepository(appDbContext);
            userProfileRepo=new UserProfileRepository(appDbContext);
            followRepo = new FollowRepository(appDbContext);
        }

        public IPostRepository PostRep { get; private set; }

        public IUserProfileRepository userProfileRepo { get; private set; }

        public IFollowRepository followRepo { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }
    }
}
