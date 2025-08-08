using Application.Abstractions.Messaging;
using Application.DTOs.Leaves;

namespace Application.Features.Leaves.LeaveTypes.Query.GetLeaveType;

public sealed record GetLeaveTypeQuery(Guid Id) : IQuery<LeaveTypeDto>;
