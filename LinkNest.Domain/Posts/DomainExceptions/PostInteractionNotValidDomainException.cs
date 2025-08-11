using LinkNest.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Domain.Posts.DomainExceptions
{
    public class PostInteractionNotValidDomainException : DomainException
    {
        public PostInteractionNotValidDomainException(string message) : base(message) { }
        public PostInteractionNotValidDomainException(string message, Exception inner) : base(message, inner) { }
    }
}
