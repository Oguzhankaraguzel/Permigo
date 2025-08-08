using SharedKernel.Abstraction;

namespace Domain.Entities.Leaves;

// Fires when a leave request is approved
public sealed record LeaveRequestApprovedEvent(LeaveRequest LeaveRequest) : IDomainEvent;
