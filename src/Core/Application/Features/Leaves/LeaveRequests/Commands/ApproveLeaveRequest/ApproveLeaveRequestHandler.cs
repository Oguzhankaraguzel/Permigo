using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Leaves;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Concrete;

namespace Application.Features.Leaves.LeaveRequests.Commands.ApproveLeaveRequest;

internal sealed class ApproveLeaveRequestHandler(
        IApplicationDbContext db,
        IUserContext userCtx)
    : ICommandHandler<ApproveLeaveRequestCommand>
{
    public async Task<Result> Handle(ApproveLeaveRequestCommand cmd, CancellationToken cancellationToken)
    {
        LeaveRequest? req = await db.LeaveRequests.Include(r => r.RequestingEmployee)
            .FirstOrDefaultAsync(r => r.Id == cmd.RequestId, cancellationToken);

        if (req is null)
        { 
            return Result.Failure(LeaveRequestError.NotFound(cmd.RequestId));
        }
        if (req.Status != LeaveStatus.Pending)
        { 
            return Result.Failure(LeaveRequestError.AlreadyProcessed(req.Id));
        }

        req.Approve(userCtx.UserId);

        LeaveEntitlement? ent = await db.LeaveEntitlements.FirstOrDefaultAsync(e =>
            e.EmployeeId == req.RequestingEmployeeId &&
            e.LeaveTypeId == req.LeaveTypeId &&
            e.Year == req.StartDate.Year, cancellationToken);

        if (ent is null)
        { 
            return Result.Failure(LeaveRequestError.NoEntitlement(req.Id));
        }
        if (ent.RemainingDays < req.DurationDays)
        { 
            return Result.Failure(LeaveRequestError.InsufficientBalance(req.Id));
        }
        ent.UsedDays += req.DurationDays;

        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
