using Application.Abstractions.Messaging;
using Application.DTOs.Leaves;
using SharedKernel.Concrete;

namespace Application.Features.Leaves.LeaveTypes.Query.ListLeaveType;

public sealed record ListLeaveTypesQuery(
    int Page = 1,
    int PageSize = 5,        
    string? Search = null    
) : IQuery<PagedResult<LeaveTypeDto>>;
