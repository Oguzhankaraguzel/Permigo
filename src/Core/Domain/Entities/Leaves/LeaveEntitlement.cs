using Domain.Abstractions;
using Domain.Entities.User;

namespace Domain.Entities.Leaves;

public class LeaveEntitlement : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public virtual AppUser Employee { get; set; }

    public Guid LeaveTypeId { get; set; }
    public virtual LeaveType LeaveType { get; set; }

    public int Year { get; set; }

    public int AllocatedDays { get; set; }

    public int UsedDays { get; set; }

    public int RemainingDays => AllocatedDays - UsedDays;
}
