namespace SharedKernel.Abstraction;

/// <summary>
/// An interface representing an entity in the domain model.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets the collection of domain events associated with this entity.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clears the domain events associated with this entity.
    /// </summary>
    void ClearDomainEvents();

    /// <summary>
    /// Raises a domain event, adding it to the collection of domain events for this entity.
    /// </summary>
    /// <param name="domainEvent"></param>
    void Raise(IDomainEvent domainEvent);

    /// <summary>
    /// Raises multiple domain events, adding them to the collection of domain events for this entity.
    /// </summary>
    /// <param name="domainEvents"></param>
    void Raise(IEnumerable<IDomainEvent> domainEvents);

    /// <summary>
    /// Raises multiple domain events, adding them to the collection of domain events for this entity.
    /// </summary>
    /// <param name="domainEvents"></param>
    void Raise(params IDomainEvent[] domainEvents);
}
