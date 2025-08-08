using Application.Abstractions.Messaging;
using Application.DTOs.Leaves;

namespace Application.Features.Leaves.LeaveEntitlements.Queries.GetEntitlement;

public sealed record GetEntitlementQuery(Guid EntitlementId) : IQuery<LeaveEntitlementDto>;
