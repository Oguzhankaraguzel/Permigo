using Application.Abstractions.Messaging;

namespace Application.Features.Leaves.LeaveRequests.Commands.CreateLeaveRequest;

public sealed record CreateLeaveRequestCommand(
    Guid LeaveTypeId,
    DateOnly StartDate,
    DateOnly EndDate,
    string? Reason
) : ICommand<Guid>;
