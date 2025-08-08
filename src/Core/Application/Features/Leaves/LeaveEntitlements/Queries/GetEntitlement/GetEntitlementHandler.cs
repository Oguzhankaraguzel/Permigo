using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.DTOs.Leaves;
using Domain.Entities.Leaves;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Concrete;

namespace Application.Features.Leaves.LeaveEntitlements.Queries.GetEntitlement;

internal sealed class GetEntitlementHandler(IApplicationDbContext db)
    : IQueryHandler<GetEntitlementQuery, LeaveEntitlementDto>
{
    public async Task<Result<LeaveEntitlementDto>> Handle(GetEntitlementQuery q, CancellationToken cancellationToken)
    {
        LeaveEntitlementDto? dto = await db.LeaveEntitlements
            .AsNoTracking()
            .Where(e => e.Id == q.EntitlementId)
            .Select(e => new LeaveEntitlementDto(
                e.Id, e.EmployeeId, e.LeaveTypeId, e.Year,
                e.AllocatedDays, e.UsedDays))
            .FirstOrDefaultAsync(cancellationToken);

        return dto is not null
            ? dto
            : Result.Failure<LeaveEntitlementDto>(EntitlementError.NotFound(q.EntitlementId));
    }
}
