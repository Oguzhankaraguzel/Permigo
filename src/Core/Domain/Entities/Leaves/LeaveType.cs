using Domain.Abstractions;
using Domain.Entities.User;

namespace Domain.Entities.Leaves;
public class LeaveType : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public bool OnlyWorkingDays { get; set; }
    public bool AllowPastPeriod { get; set; }               
    public int? MaxDaysPerRequest { get; set; }              
    public Gender? ApplicableGender { get; set; }               

    #region Navigation
    public virtual ICollection<LeaveRequest> Requests { get; set; } = [];
    public virtual ICollection<LeaveEntitlement> Entitlements { get; set; } = [];
    #endregion
}
