using SharedKernel.Abstraction;

namespace Domain.Entities.User;

public sealed record UserRegisteredDomainEvent(Guid UserId) : IDomainEvent;
