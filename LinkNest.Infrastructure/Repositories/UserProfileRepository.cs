using ApartmentBooking.Domain.Users;
using LinkNest.Domain.UserProfiles;
using LinkNest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkNest.Infrastructure.Repositories
{
    internal class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext appDbContext;
        public UserProfileRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task AddAsync(UserProfile userProfile)
        {
            await appDbContext.Set<UserProfile>().AddAsync(userProfile);

        }

        public async Task<UserProfile> GetByIdAsync(Guid userProfileId)
        {
           return await appDbContext.Set<UserProfile>().FirstOrDefaultAsync(u=>u.Guid==userProfileId);
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await appDbContext.Set<UserProfile>().AnyAsync(u => u.Email == new UserProfileEmail(email));
        }

        public async Task<bool> IsEmailExist(string email,string except)
        {
            return await appDbContext.Set<UserProfile>().AnyAsync(u => u.Email == new UserProfileEmail(email) && u.Email != new UserProfileEmail(except) );
        }

        public void Remove(UserProfile userProfile)
        {
            appDbContext.Set<UserProfile>().Remove(userProfile);
        }

        public void Update(UserProfile userProfile)
        {
             appDbContext.Set<UserProfile>().Update(userProfile);
        }
    }
}
