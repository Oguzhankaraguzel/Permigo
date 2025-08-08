using System.Collections.Generic;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.DTOs.Leaves;
using Domain.Entities.Leaves;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedKernel.Concrete;
using SharedKernel.Extensions.Linq;

namespace Application.Features.Leaves.LeaveRequests.Queries.ListLeaveRequests;
internal sealed class ListLeaveRequestsHandler(
        IApplicationDbContext db,
        IUserContext userContext,
        UserManager<AppUser> userManager)
    : IQueryHandler<ListLeaveRequestsQuery, IReadOnlyCollection<LeaveRequestDto>>
{
    public async Task<Result<IReadOnlyCollection<LeaveRequestDto>>> Handle(
        ListLeaveRequestsQuery request,
        CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(userContext.UserId.ToString());
        if (user == null)
        {
            return Result.Failure<IReadOnlyCollection<LeaveRequestDto>>(
                UserError.NotFound(userContext.UserId));
        }

        IQueryable<LeaveRequest> q = db.LeaveRequests
            .AsNoTracking()
            .Include(r => r.LeaveType)
            .Include(r => r.RequestingEmployee)
            .WhereIf(!string.IsNullOrWhiteSpace(request.Status), r => r.Status.ToString() == request.Status)
            .WhereIf(await userManager.IsInRoleAsync(user,Roles.Employee),x => x.RequestingEmployeeId == user.Id);

        List<LeaveRequestDto> list = await q
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new LeaveRequestDto(
                r.Id,
                r.RequestingEmployeeId,
                r.RequestingEmployee.FullName,
                r.LeaveTypeId,
                r.LeaveType.Name,
                r.StartDate,
                r.EndDate,
                r.DurationDays,
                r.Status.ToString()))
            .ToListAsync(cancellationToken);

        return list;                        
    }
}
