using SharedKernel.Abstraction;

namespace SharedKernel.Concrete;

public abstract class Entity : IEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void Raise(IEnumerable<IDomainEvent> domainEvents) => _domainEvents.AddRange(domainEvents);
    public void Raise(params IDomainEvent[] domainEvents) => Raise(domainEvents.AsEnumerable());
}
