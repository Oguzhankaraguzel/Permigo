using FluentValidation;

namespace Application.Features.Leaves.LeaveTypes.Query.GetLeaveType;

public sealed class GetLeaveTypeValidator : AbstractValidator<GetLeaveTypeQuery>
{
    public GetLeaveTypeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
