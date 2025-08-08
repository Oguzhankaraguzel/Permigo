using Domain.Entities.Leaves;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Abstraction;

namespace Domain.Entities.User;

public class AppUser : IdentityUser<Guid>, IEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public Gender Gender { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    #region Domain Events
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void Raise(IEnumerable<IDomainEvent> domainEvents) => _domainEvents.AddRange(domainEvents);
    public void Raise(params IDomainEvent[] domainEvents) => Raise(domainEvents.AsEnumerable());
    #endregion
}
