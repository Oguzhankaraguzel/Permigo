using FluentValidation;

namespace Application.Features.Leaves.LeaveEntitlements.Commands.AssignEntitlement;

public sealed class AssignEntitlementValidator : AbstractValidator<AssignEntitlementCommand>
{
    public AssignEntitlementValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.LeaveTypeId).NotEmpty();
        RuleFor(x => x.Year).InclusiveBetween(DateTime.UtcNow.Year - 1, DateTime.UtcNow.Year + 1);
        RuleFor(x => x.AllocatedDays).InclusiveBetween(1, 60);
    }
}
