using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Leaves;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Concrete;

namespace Application.Features.Leaves.LeaveRequests.Commands.RejectLeaveRequest;

internal sealed class RejectLeaveRequestHandler(
        IApplicationDbContext db,
        IUserContext userCtx)
    : ICommandHandler<RejectLeaveRequestCommand>
{
    public async Task<Result> Handle(RejectLeaveRequestCommand cmd, CancellationToken cancellationToken)
    {
        LeaveRequest? req = await db.LeaveRequests.Include(r => r.RequestingEmployee).FirstOrDefaultAsync(r => r.Id == cmd.RequestId, cancellationToken);
        if (req is null)
        { 
            return Result.Failure(LeaveRequestError.NotFound(cmd.RequestId));
        }
        if (req.Status != LeaveStatus.Pending)
        { 
            return Result.Failure(LeaveRequestError.AlreadyProcessed(req.Id));
        }
        req.Reject(userCtx.UserId, cmd.Reason);
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
