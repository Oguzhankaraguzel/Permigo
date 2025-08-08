using FluentValidation;

namespace Application.Features.Leaves.LeaveRequests.Commands.CreateLeaveRequest;

public sealed class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestValidator()
    {
        RuleFor(x => x.LeaveTypeId).NotEmpty();

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.Date))
            .WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be after start date.");
    }
}
