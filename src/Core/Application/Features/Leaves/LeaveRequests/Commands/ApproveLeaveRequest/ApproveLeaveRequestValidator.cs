using FluentValidation;

namespace Application.Features.Leaves.LeaveRequests.Commands.ApproveLeaveRequest;

public sealed class ApproveLeaveRequestValidator : AbstractValidator<ApproveLeaveRequestCommand>
{
    public ApproveLeaveRequestValidator() =>
        RuleFor(x => x.RequestId).NotEmpty();
}
