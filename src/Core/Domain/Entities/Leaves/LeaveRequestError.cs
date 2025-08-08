using SharedKernel.Concrete;

namespace Domain.Entities.Leaves;

public static class LeaveRequestError
{
    public static Error NotFound(Guid requestId) => Error.NotFound(
        "LeaveRequest.NotFound",
        $"The leave request with ID '{requestId}' was not found.");

    public static Error AlreadyProcessed(Guid requestId) => Error.Conflict(
        "LeaveRequest.AlreadyProcessed",
        $"The leave request ({requestId}) has already been processed and its status cannot be changed.");

    public static Error NoEntitlement(Guid requestId) => Error.Failure(
        "LeaveRequest.NoEntitlementFound",
        $"The employee associated with leave request ({requestId}) does not have an active leave entitlement for this period.");

    public static Error InsufficientBalance(Guid requestId) => Error.Failure(
        "LeaveRequest.InsufficientBalance",
        $"The employee has an insufficient leave balance to fulfill this request ({requestId}).");

    public static Error LeaveTypeNotFound(Guid id) => Error.NotFound(
    "LeaveRequest.LeaveTypeNotFound",
    $"Leave type '{id}' was not found.");

    public static Error GenderNotApplicable(string leaveType, string? empGender) => Error.Failure(
        "LeaveRequest.GenderNotApplicable",
        $"The '{leaveType}' leave is not applicable for the gender '{empGender ?? "undefined"}'.");

    public static Error ExceedMaxDays(string leaveType, int max) => Error.Failure(
        "LeaveRequest.ExceedsMaxDays",
        $"A maximum of {max} days can be requested for '{leaveType}'.");

    public static Error PastPeriodNotAllowed() => Error.Failure(
        "LeaveRequest.PastPeriodNotAllowed",
        "Leave cannot be requested for a past period.");

    public static Error NoWorkingDays() => Error.Failure(
        "LeaveRequest.NoWorkingDays",
        "No working days were found in the selected date range.");

    public static Error EntitlementExceeded(int remainingDays) => Error.Failure(
    "LeaveRequest.EntitlementExceeded",
    $"The requested number of days exceeds the available entitlement. Remaining leave days: {remainingDays}.");

    public static Error Overlaps(DateOnly start, DateOnly end) => Error.Conflict(
    "LeaveRequest.Overlaps",
    $"The requested period ({start:dd.MM.yyyy} – {end:dd.MM.yyyy}) conflicts with an existing leave request.");
}
