using FluentValidation;

namespace Application.Features.Leaves.LeaveTypes.Command.Create;
public sealed class CreateLeaveTypeValidator : AbstractValidator<CreateLeaveTypeCommand>
{
    public CreateLeaveTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(128);
    }
}
