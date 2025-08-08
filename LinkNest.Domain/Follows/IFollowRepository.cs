namespace LinkNest.Domain.Follows
{
    public interface IFollowRepository
    {
        Task<Follow> GetFollowAsync(Guid followerId, Guid followeeId);
        Task AddAsync(Follow follow);
        void Remove(Follow follow);
        Task<bool> IsFollowingAsync(Guid followerId, Guid followeeId);
    }
}
