using Application.Abstractions.Messaging;

namespace Application.Features.Leaves.LeaveRequests.Commands.ApproveLeaveRequest;

public sealed record ApproveLeaveRequestCommand(Guid RequestId)
    : ICommand;
