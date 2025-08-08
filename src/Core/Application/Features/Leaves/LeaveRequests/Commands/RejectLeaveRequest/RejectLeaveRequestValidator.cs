using FluentValidation;

namespace Application.Features.Leaves.LeaveRequests.Commands.RejectLeaveRequest;

public sealed class RejectLeaveRequestValidator : AbstractValidator<RejectLeaveRequestCommand>
{
    public RejectLeaveRequestValidator()
    {
        RuleFor(x => x.RequestId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(512);
    }
}
