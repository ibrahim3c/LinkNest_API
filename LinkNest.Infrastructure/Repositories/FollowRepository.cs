using LinkNest.Domain.Follows;
using LinkNest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkNest.Infrastructure.Repositories
{
    internal class FollowRepository : IFollowRepository
    {
        private readonly AppDbContext _appDbContext;

        public FollowRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Follow follow)
        {
            await _appDbContext.Set<Follow>().AddAsync(follow);
        }

        public async Task<Follow> GetFollowAsync(Guid followerId, Guid followeeId)
        {
            return await _appDbContext.Set<Follow>()
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);

        }

        public async Task<bool> IsFollowingAsync(Guid followerId, Guid followeeId)
        {
            return await _appDbContext.Set<Follow>()
                          .AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
        }

        public void Remove(Follow follow)
        {
             _appDbContext.Set<Follow>().Remove(follow);
        }
    }
}
