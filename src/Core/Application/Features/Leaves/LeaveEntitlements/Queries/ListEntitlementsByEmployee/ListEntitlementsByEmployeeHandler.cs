using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.DTOs.Leaves;
using Domain.Entities.Leaves;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Concrete;

namespace Application.Features.Leaves.LeaveEntitlements.Queries.ListEntitlementsByEmployee;
internal sealed class ListEntitlementsByEmployeeHandler(IApplicationDbContext db)
    : IQueryHandler<ListEntitlementsByEmployeeQuery, IReadOnlyCollection<LeaveEntitlementDto>>
{
    public async Task<Result<IReadOnlyCollection<LeaveEntitlementDto>>> Handle(ListEntitlementsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        IQueryable<LeaveEntitlement> query = db.LeaveEntitlements
            .AsNoTracking()
            .Where(e => e.EmployeeId == request.EmployeeId);

        if (request.Year.HasValue)
        {
            query = query.Where(e => e.Year == request.Year);
        }
        return await query
            .OrderBy(e => e.Year)
            .ThenBy(e => e.LeaveTypeId)
            .Select(e => new LeaveEntitlementDto(
                e.Id, e.EmployeeId, e.LeaveTypeId, e.Year,
                e.AllocatedDays, e.UsedDays))
            .ToListAsync(cancellationToken);
    }
}
