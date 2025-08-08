using Domain.Entities.Leaves;
using Domain.Entities.User;
using Domain.Entities.Calender;

namespace Application;

/// <summary>
/// Central place for constants & default seed data.
/// </summary>
public static class DomainSeeds
{
    // ── Global ──────────────────────────────────────────────────────────────────
    public const string InitialPassword = "P@ssword123";
    public const int CurrentYear = 2025;

    // ── Roles ───────────────────────────────────────────────────────────────────
    public static readonly Guid SuperAdminRoleId = Guid.Parse("D788C6DA-B626-4EFD-8C49-6235595BC8C8");
    public static readonly Guid ManagerRoleId = Guid.Parse("7B7B3EFA-67C4-4F04-962E-90E9F9242D47");
    public static readonly Guid EmployeeRoleId = Guid.Parse("361A926E-6C86-4A90-A619-B2D5B981B42D");

    public static AppRole SuperAdminRole() => new() { Id = SuperAdminRoleId, Name = Roles.SuperAdmin, NormalizedName = Roles.SuperAdmin.ToUpperInvariant() };
    public static AppRole ManagerRole() => new() { Id = ManagerRoleId, Name = Roles.Manager, NormalizedName = Roles.Manager.ToUpperInvariant() };
    public static AppRole EmployeeRole() => new() { Id = EmployeeRoleId, Name = Roles.Employee, NormalizedName = Roles.Employee.ToUpperInvariant() };

    // ── Users ───────────────────────────────────────────────────────────────────
    public static readonly Guid SuperAdminId = Guid.Parse("A9695636-1B4B-4D92-B3C5-98991D71DF6F");
    public static readonly Guid Manager1Id = Guid.Parse("9AF5F80A-8E8F-4F1D-9B1D-111111111111");
    public static readonly Guid Manager2Id = Guid.Parse("9AF5F80A-8E8F-4F1D-9B1D-222222222222");
    public static readonly Guid[] EmployeeIds =
    {
        Guid.Parse("E1111111-1111-1111-1111-111111111111"),
        Guid.Parse("E2222222-2222-2222-2222-222222222222"),
        Guid.Parse("E3333333-3333-3333-3333-333333333333"),
        Guid.Parse("E4444444-4444-4444-4444-444444444444"),
        Guid.Parse("E5555555-5555-5555-5555-555555555555"),
        Guid.Parse("E6666666-6666-6666-6666-666666666666")
    };

    // ── LeaveTypes ───────────────────────────────────────────────────────────────
    public static readonly Guid AnnualLeaveId = Guid.Parse("AA111111-1111-1111-1111-111111111111");
    public static readonly Guid PaternityLeaveId = Guid.Parse("AA222222-2222-2222-2222-222222222222");
    public static readonly Guid SickLeaveId = Guid.Parse("AA333333-3333-3333-3333-333333333333");

    public static LeaveType AnnualLeave() => new()
    {
        Id = AnnualLeaveId,
        Name = "Annual Leave",
        OnlyWorkingDays = true,
        AllowPastPeriod = false,
        ApplicableGender = null,
        CreateUserId = SuperAdminId,
        CreatedAt = DateTime.UtcNow
    };

    public static LeaveType PaternityLeave() => new()
    {
        Id = PaternityLeaveId,
        Name = "Paternity Leave",
        OnlyWorkingDays = true,
        AllowPastPeriod = false,
        ApplicableGender = Gender.Male,
        CreateUserId = SuperAdminId,
        CreatedAt = DateTime.UtcNow
    };

    public static LeaveType SickLeave() => new()
    {
        Id = SickLeaveId,
        Name = "Sick Leave",
        OnlyWorkingDays = false,
        AllowPastPeriod = true,
        ApplicableGender = null,
        CreateUserId = SuperAdminId,
        CreatedAt = DateTime.UtcNow
    };

    // ── LeaveEntitlements ───────────────────────────────────────────────────────
    /// <summary>
    /// Creates default annual leave entitlements (20 days) for each employee for the specified year.
    /// </summary>
    public static IEnumerable<LeaveEntitlement> DefaultLeaveEntitlements(int year = CurrentYear)
    {
        foreach (Guid empId in EmployeeIds)
        {
            yield return new LeaveEntitlement
            {
                Id = Guid.NewGuid(),
                EmployeeId = empId,
                LeaveTypeId = AnnualLeaveId,
                Year = year,
                AllocatedDays = 20,
                UsedDays = 0,
                CreateUserId = SuperAdminId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }

    // ── WorkingDays (optional) ──────────────────────────────────────────────────
    /// <summary>
    /// Returns all days of the given month with weekend distinction.
    /// </summary>
    public static IEnumerable<WorkingDay> WorkingDaysForMonth(int year, int month)
    {
        int daysInMonth = DateTime.DaysInMonth(year, month);

        for (int d = 1; d <= daysInMonth; d++)
        {
            var date = new DateOnly(year, month, d);
            yield return new WorkingDay
            {
                Id = Guid.NewGuid(),
                Date = date,
                IsWorkingDay = date.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday),
                CreatedAt = DateTime.UtcNow,
                CreateUserId= SuperAdminId,
            };
        }
    }
}
