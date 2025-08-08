using FluentValidation;

namespace Application.Features.Leaves.LeaveEntitlements.Commands.AdjustEntitlement;

public sealed class AdjustEntitlementValidator : AbstractValidator<AdjustEntitlementCommand>
{
    public AdjustEntitlementValidator()
    {
        RuleFor(x => x.EntitlementId).NotEmpty();
        RuleFor(x => x.AllocatedDays).GreaterThanOrEqualTo(0).When(x => x.AllocatedDays.HasValue);
        RuleFor(x => x.UsedDays).GreaterThanOrEqualTo(0).When(x => x.UsedDays.HasValue);
    }
}
