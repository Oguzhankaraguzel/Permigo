namespace Application.DTOs.Leaves;

public sealed record LeaveEntitlementDto(
    Guid Id,
    Guid EmployeeId,
    Guid LeaveTypeId,
    int Year,
    int AllocatedDays,
    int UsedDays
)
{
    public int RemainingDays => AllocatedDays - UsedDays;
}
