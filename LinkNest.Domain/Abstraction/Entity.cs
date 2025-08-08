namespace LinkNest.Domain.Abstraction
{
    public abstract class Entity
    {
        public Guid Guid { get; init; }
        private List<IDomainEvent> _events= new();
        protected Entity(Guid guid)
        {
            Guid = guid;
        }
        protected Entity() { }

        public IReadOnlyList<IDomainEvent> GetDomainEvents() => _events;
        public void RaiseDomainEvent(IDomainEvent domainEvent) => _events.Add(domainEvent);
        public void ClearDomainEvents()=>_events.Clear();
       

    }
}
