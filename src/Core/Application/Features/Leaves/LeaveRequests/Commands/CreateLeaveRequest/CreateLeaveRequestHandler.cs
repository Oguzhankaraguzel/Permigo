using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Leaves;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Concrete;

namespace Application.Features.Leaves.LeaveRequests.Commands.CreateLeaveRequest;

internal sealed class CreateLeaveRequestHandler(
        IApplicationDbContext db,
        IUserContext userCtx,
        UserManager<AppUser> userManager)
    : ICommandHandler<CreateLeaveRequestCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateLeaveRequestCommand cmd, CancellationToken cancellationToken)
    {
        Guid empId = userCtx.UserId;

        LeaveType? leaveType = await db.LeaveTypes.FindAsync([cmd.LeaveTypeId], cancellationToken);
        if (leaveType is null)
        {
            return Result.Failure<Guid>(LeaveRequestError.LeaveTypeNotFound(cmd.LeaveTypeId));
        }

        bool overlap = await db.LeaveRequests.AnyAsync(r =>
            r.RequestingEmployeeId == empId &&
            r.Status != LeaveStatus.Rejected &&
            r.StartDate <= cmd.EndDate &&
            r.EndDate >= cmd.StartDate, cancellationToken);

        if (overlap)
        {
            return Result.Failure<Guid>(LeaveRequestError.Overlaps(cmd.StartDate, cmd.EndDate));
        }

        AppUser? employee = await userManager.FindByIdAsync(empId.ToString());

        LeaveEntitlement? entitlement = await db.LeaveEntitlements.FirstOrDefaultAsync(e =>
            e.EmployeeId == empId &&
            e.LeaveTypeId == leaveType.Id &&
            e.Year == DateTime.UtcNow.Year, cancellationToken);

        int totalDays = cmd.EndDate.DayNumber - cmd.StartDate.DayNumber;
        IEnumerable<DateOnly> rangeDays = Enumerable.Range(0, totalDays + 1)
                                  .Select(o => cmd.StartDate.AddDays(o));

        List<DateOnly> effectiveDays = leaveType.OnlyWorkingDays
            ? await db.WorkingDays.Where(w => w.IsWorkingDay && rangeDays.Contains(w.Date))
                                  .Select(w => w.Date)
                                  .ToListAsync(cancellationToken)
            : rangeDays.ToList();

        if (!effectiveDays.Any())
        {
            return Result.Failure<Guid>(LeaveRequestError.NoWorkingDays());
        }

        int requested = effectiveDays.Count;

        Error? ruleError = CheckRules(leaveType, employee, entitlement, requested, cmd.StartDate);
        if (ruleError is not null)
        {
            return Result.Failure<Guid>(ruleError);
        }

        var entity = new LeaveRequest
        {
            Id = Guid.NewGuid(),
            RequestingEmployeeId = empId,
            LeaveTypeId = leaveType.Id,
            StartDate = cmd.StartDate,
            EndDate = cmd.EndDate,
            DurationDays = requested,
            Reason = cmd.Reason,
            CreatedAt = DateTime.UtcNow,
            CreateUserId = empId,
            CreateUser = employee!,
        };

        await db.LeaveRequests.AddAsync(entity, cancellationToken);
        entitlement!.UsedDays += requested;
        await db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    private static Error? CheckRules(
        LeaveType leaveType,
        AppUser? employee,
        LeaveEntitlement? entitlement,
        int requestedDays,
        DateOnly startDate)
    {
        if (!leaveType.AllowPastPeriod && startDate < DateOnly.FromDateTime(DateTime.UtcNow.Date))
        {
            return LeaveRequestError.PastPeriodNotAllowed();
        }

        if (leaveType.MaxDaysPerRequest is int max && requestedDays > max)
        {
            return LeaveRequestError.ExceedMaxDays(leaveType.Name, max);
        }

        if (leaveType.ApplicableGender is not null && employee?.Gender != leaveType.ApplicableGender)
        {
            return LeaveRequestError.GenderNotApplicable(leaveType.Name, employee?.Gender.ToString());
        }

        if (entitlement is null || entitlement.RemainingDays < requestedDays)
        {
            return LeaveRequestError.EntitlementExceeded(entitlement?.RemainingDays ?? 0);
        }

        return null;
    }
}
