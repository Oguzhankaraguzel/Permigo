using SharedKernel.Concrete;

namespace Domain.Entities.Leaves;

public static class EntitlementError
{
    public static Error AlreadyExists(Guid EmployeeId,int year) => Error.Conflict(
        "LeaveEntitlement.AlreadyExists",
        $"A leave entitlement for employee ({EmployeeId}) for the year {year} already exists.");

    public static Error NotFound(Guid entitlementId) => Error.NotFound(
    "LeaveEntitlement.NotFound",
    $"The leave entitlement with ID '{entitlementId}' was not found.");

    public static Error NegativeBalance(Guid employeeId) => Error.Failure(
    "Leave.InsufficientBalance",
    $"Insufficient leave balance. The employee ({employeeId}) does not have enough available leave for this operation.");
}
