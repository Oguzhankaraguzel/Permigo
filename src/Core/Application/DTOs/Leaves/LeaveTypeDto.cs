using Domain.Entities.User;

namespace Application.DTOs.Leaves;

public sealed record LeaveTypeDto(
    Guid Id,
    string Name,
    bool OnlyWorkingDays,
    bool AllowPastPeriod,
    Gender? ApplicableGender
);
