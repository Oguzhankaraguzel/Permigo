using FluentValidation;

namespace Application.Features.Leaves.LeaveTypes.Query.ListLeaveType;

public sealed class ListLeaveTypesValidator : AbstractValidator<ListLeaveTypesQuery>
{
    public ListLeaveTypesValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}
