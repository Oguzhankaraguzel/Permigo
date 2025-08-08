using SharedKernel.Abstraction;

namespace Domain.Entities.Leaves;

// Fires when an employee submits a new leave request
public sealed record LeaveRequestCreatedEvent(LeaveRequest LeaveRequest) : IDomainEvent;
