using LinkNest.Domain.Abstraction;

namespace LinkNest.Domain.Follows.DomainExceptions
{
    public class FollowRequestNotValidDomainException:DomainException
    {
        public FollowRequestNotValidDomainException(string message) : base(message)
        {
        }
        public FollowRequestNotValidDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
