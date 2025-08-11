using LinkNest.Domain.Abstraction;

namespace LinkNest.Domain.Posts.DomainExceptions
{
    public class PostCommentNotValidDomainException : DomainException
    {
        public PostCommentNotValidDomainException(string msg) : base(msg)
        {
        }
        public PostCommentNotValidDomainException(string msg, Exception inner) : base(msg, inner)
        {
        }
    }
}
