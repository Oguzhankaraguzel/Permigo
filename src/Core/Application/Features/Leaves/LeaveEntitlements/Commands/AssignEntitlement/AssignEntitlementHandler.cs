using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Leaves;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Concrete;

namespace Application.Features.Leaves.LeaveEntitlements.Commands.AssignEntitlement;
internal sealed class AssignEntitlementHandler(
        IApplicationDbContext db,
        IUserContext userCtx)
    : ICommandHandler<AssignEntitlementCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        AssignEntitlementCommand cmd, CancellationToken cancellationToken)
    {
        bool exists = await db.LeaveEntitlements
            .AnyAsync(e => e.EmployeeId == cmd.EmployeeId &&
                           e.LeaveTypeId == cmd.LeaveTypeId &&
                           e.Year == cmd.Year, cancellationToken);

        if (exists)
        { 
            return Result.Failure<Guid>(EntitlementError.AlreadyExists(cmd.EmployeeId, cmd.Year));
        }
        var entity = new LeaveEntitlement
        {
            Id = Guid.NewGuid(),
            EmployeeId = cmd.EmployeeId,
            LeaveTypeId = cmd.LeaveTypeId,
            Year = cmd.Year,
            AllocatedDays = cmd.AllocatedDays,
            UsedDays = 0,
            CreateUserId = userCtx.UserId,
            CreatedAt = DateTime.UtcNow
        };

        await db.LeaveEntitlements.AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
