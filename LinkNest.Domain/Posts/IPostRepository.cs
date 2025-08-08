
using System.Linq.Expressions;

namespace LinkNest.Domain.Posts
{
    public interface IPostRepository
    {
        // post
        Task AddAsync(Post post);
        void Update(Post post);
        void Delete(Post post);
        Task<Post> GetByIdAsync(Guid PostId);
        Task<Post> GetByIdAsync(Guid postId, params Expression<Func<Post, object>>[] includes);

        //comment
        Task AddCommentAsync(PostComment comment);
        void UpdateComment(PostComment comment);
        void DeleteComment(PostComment comment);
        Task<PostComment> GetCommentByIdAsync(Guid commentId);

        //interaction
        Task AddInteractionAsync(PostInteraction interaction);
        void DeleteInteraction(PostInteraction interaction);
        Task<PostInteraction> GetInteractionByIdAsync(Guid interactionId);



    }
}
