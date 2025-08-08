using Application.Abstractions.Messaging;

namespace Application.Features.Leaves.LeaveEntitlements.Commands.AdjustEntitlement;
/// <summary>
/// Adjusts allocated / used days of an existing record.
/// </summary>
public sealed record AdjustEntitlementCommand(
    Guid EntitlementId,
    int? AllocatedDays,
    int? UsedDays
) : ICommand;
