using System.Globalization;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Leaves;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Concrete;

namespace Application.Features.Leaves.LeaveTypes.Command.Create;
internal sealed class CreateLeaveTypeHandler(
        IApplicationDbContext context,
        IUserContext userContext)
    : ICommandHandler<CreateLeaveTypeCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateLeaveTypeCommand request,
        CancellationToken cancellationToken)
    {
        bool exists = await context.LeaveTypes
    .AnyAsync(l => EF.Functions.Like(l.Name, request.Name.Trim()), cancellationToken);

        if (exists)
        { 
            return Result.Failure<Guid>(LeaveError.DuplicateLeaveType(request.Name));
        }

        var entity = new LeaveType
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            OnlyWorkingDays = request.OnlyWorkingDays,
            AllowPastPeriod = request.AllowPastPeriod,
            ApplicableGender = request.ApplicableGender,
            CreateUserId = userContext.UserId,
            CreatedAt = DateTime.UtcNow
        };

        // 3. Persist
        await context.LeaveTypes.AddAsync(entity,cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
