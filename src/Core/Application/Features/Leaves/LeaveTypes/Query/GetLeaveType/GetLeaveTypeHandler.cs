using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.DTOs.Leaves;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Concrete;

namespace Application.Features.Leaves.LeaveTypes.Query.GetLeaveType;
internal sealed class GetLeaveTypeHandler(
        IApplicationDbContext repo)
    : IQueryHandler<GetLeaveTypeQuery, LeaveTypeDto>
{
    public async Task<Result<LeaveTypeDto>> Handle(GetLeaveTypeQuery request, CancellationToken cancellationToken)
    {
        LeaveTypeDto? entity = await repo.LeaveTypes
            .AsQueryable()
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new LeaveTypeDto(x.Id, x.Name, x.OnlyWorkingDays, x.AllowPastPeriod, x.ApplicableGender))
            .FirstOrDefaultAsync(cancellationToken);

        return entity!;
    }
}
