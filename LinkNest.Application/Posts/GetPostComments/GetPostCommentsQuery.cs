using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Posts.GetPostComments
{
    public  record GetPostCommentsQuery(Guid postId):IQuery<GetPostCommentsResponse>;
}
