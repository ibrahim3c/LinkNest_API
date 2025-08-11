using LinkNest.Domain.Abstraction;

namespace LinkNest.Domain.Posts.DomainExceptions
{
    public class PostNotValidDomainException : DomainException
    {
        public PostNotValidDomainException(string message) : base(message)
        {
        }
        public PostNotValidDomainException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
