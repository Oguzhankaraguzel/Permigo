using Application.Abstractions.Messaging;
using Application.DTOs.Leaves;

namespace Application.Features.Leaves.LeaveEntitlements.Queries.ListEntitlementsByEmployee;

public sealed record ListEntitlementsByEmployeeQuery(Guid EmployeeId, int? Year = null)
    : IQuery<IReadOnlyCollection<LeaveEntitlementDto>>;
