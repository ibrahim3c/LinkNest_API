namespace LinkNest.Domain.Abstraction
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string msg) : base(msg) { }
        protected DomainException(string msg, Exception inner) : base(msg, inner) { }
    }
}
