using SharedKernel.Abstraction;

namespace Domain.Entities.Leaves;

// Fires when a leave request is rejected
public sealed record LeaveRequestRejectedEvent(LeaveRequest LeaveRequest) : IDomainEvent;
