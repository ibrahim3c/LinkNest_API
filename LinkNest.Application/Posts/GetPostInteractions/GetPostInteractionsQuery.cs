using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Posts.GetPostInteractions
{
    public record GetPostInteractionsQuery(Guid postId):IQuery<GetPostInteractionsResponse>;
}
