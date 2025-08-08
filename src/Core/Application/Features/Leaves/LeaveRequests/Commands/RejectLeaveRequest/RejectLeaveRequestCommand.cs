using Application.Abstractions.Messaging;

namespace Application.Features.Leaves.LeaveRequests.Commands.RejectLeaveRequest;

public sealed record RejectLeaveRequestCommand(Guid RequestId, string Reason)
    : ICommand;
