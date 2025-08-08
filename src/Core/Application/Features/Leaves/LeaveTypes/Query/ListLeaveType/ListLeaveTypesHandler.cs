using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.DTOs.Leaves;
using Domain.Entities.Leaves;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Concrete;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Features.Leaves.LeaveTypes.Query.ListLeaveType;
internal sealed class ListLeaveTypesHandler(
        IApplicationDbContext repo)
    : IQueryHandler<ListLeaveTypesQuery, PagedResult<LeaveTypeDto>>
{
    public async Task<Result<PagedResult<LeaveTypeDto>>> Handle(ListLeaveTypesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<LeaveType> baseQ = repo.LeaveTypes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        { 
            baseQ = baseQ.Where(l => EF.Functions.Like(l.Name, $"%{request.Search.Trim()}%"));
        }
        int total = await baseQ.CountAsync(cancellationToken);

        List<LeaveTypeDto> items = await baseQ
            .OrderBy(l => l.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(l => new LeaveTypeDto(
                l.Id, l.Name, l.OnlyWorkingDays, l.AllowPastPeriod, l.ApplicableGender))
            .ToListAsync(cancellationToken);

        return new PagedResult<LeaveTypeDto>(items, request.Page, request.PageSize, total);
    }
}
