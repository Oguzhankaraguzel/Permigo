using Application.Abstractions.Messaging;

namespace Application.Features.Leaves.LeaveEntitlements.Commands.AssignEntitlement;

public sealed record AssignEntitlementCommand(
    Guid EmployeeId,
    Guid LeaveTypeId,
    int Year,
    int AllocatedDays
) : ICommand<Guid>;
