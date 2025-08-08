using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Leaves;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Concrete;

namespace Application.Features.Leaves.LeaveEntitlements.Commands.AdjustEntitlement;
internal sealed class AdjustEntitlementHandler(IApplicationDbContext db)
    : ICommandHandler<AdjustEntitlementCommand>
{
    public async Task<Result> Handle(AdjustEntitlementCommand cmd, CancellationToken cancellationToken)
    {
        LeaveEntitlement? entity = await db.LeaveEntitlements
            .FirstOrDefaultAsync(e => e.Id == cmd.EntitlementId, cancellationToken);

        if (entity is null)
        { 
            return Result.Failure(EntitlementError.NotFound(cmd.EntitlementId));
        }
        if (cmd.AllocatedDays.HasValue)
        { 
            entity.AllocatedDays = cmd.AllocatedDays.Value;
        }
        if (cmd.UsedDays.HasValue)
        {
            if (cmd.UsedDays.Value > entity.AllocatedDays)
            { 
                return Result.Failure(EntitlementError.NegativeBalance(entity.Id));
            }
            entity.UsedDays = cmd.UsedDays.Value;
        }

        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
