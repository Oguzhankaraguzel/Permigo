using Application.Abstractions.Messaging;
using Domain.Entities.User;

namespace Application.Features.Leaves.LeaveTypes.Command.Create;

public sealed record CreateLeaveTypeCommand(
    string Name,
    bool OnlyWorkingDays,
    bool AllowPastPeriod,
    Gender? ApplicableGender
) : ICommand<Guid>;
