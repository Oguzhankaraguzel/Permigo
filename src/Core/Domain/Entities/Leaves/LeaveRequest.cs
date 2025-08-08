using System.ComponentModel.DataAnnotations.Schema;
using Domain.Abstractions;
using Domain.Entities.User;

namespace Domain.Entities.Leaves;

public class LeaveRequest : BaseEntity
{
    public Guid RequestingEmployeeId { get; set; }
    public Guid LeaveTypeId { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int DurationDays { get; set; }     

    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
    public Guid? ApproverId { get; set; }
    public string? Reason { get; set; }

    #region Domain Behaviours
    public void Approve(Guid approverId)
    {
        Status = LeaveStatus.Approved;
        ApproverId = approverId;
        Raise(new LeaveRequestApprovedEvent(this));
    }

    public void MarkCreated() => Raise(new LeaveRequestCreatedEvent(this));

    public void Reject(Guid approverId, string reason)
    {
        Status = LeaveStatus.Rejected;
        ApproverId = approverId;
        Reason = reason;
        Raise(new LeaveRequestRejectedEvent(this));
    }
    #endregion

    #region Navigation
    public virtual LeaveType LeaveType { get; set; }
    public virtual AppUser RequestingEmployee { get; set; }
    public virtual AppUser? Approver { get; set; }
    #endregion
}
