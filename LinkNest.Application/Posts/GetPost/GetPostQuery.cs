using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Posts.GetPost
{
    public sealed record GetPostQuery(Guid PostId):IQuery<GetPostResponse>;
}
