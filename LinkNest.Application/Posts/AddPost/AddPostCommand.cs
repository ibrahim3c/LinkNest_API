using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Posts;

namespace LinkNest.Application.Posts.AddPost
{
    public record AddPostCommand(string Content,  string ImageUrl , Guid UserProfileId) :ICommand<Guid>;
}
