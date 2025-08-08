using Domain.Entities.Calender;
using Domain.Entities.Leaves;
using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<AppUser> AppUsers { get; }
    DbSet<AppRole> AppRoles { get; }
    DbSet<WorkingDay> WorkingDays { get; }
    DbSet<LeaveRequest> LeaveRequests { get; }
    DbSet<LeaveType> LeaveTypes { get; }
    DbSet<LeaveEntitlement> LeaveEntitlements { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
