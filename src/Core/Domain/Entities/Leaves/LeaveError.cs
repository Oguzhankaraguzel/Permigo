using SharedKernel.Concrete;

namespace Domain.Entities.Leaves;

public static class LeaveError
{
    public static Error DuplicateLeaveType(string name) => Error.Conflict(
        "LeaveType.Duplicate",
        $"Leave Type with the name '{name}' already exists.");
}
