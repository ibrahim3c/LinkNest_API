using LinkNest.Domain.Abstraction;

namespace LinkNest.Domain.UserProfiles.DomainExceptions
{
    public class UserProfileNotValidException : DomainException
    {
        internal UserProfileNotValidException(string message) : base(message) { }
        internal UserProfileNotValidException(string message, Exception inner) : base(message, inner) { }
    }
}
