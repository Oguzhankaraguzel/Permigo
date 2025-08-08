using Application;
using Domain.Entities.Leaves;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Context;

/// <summary>
/// Seeds default roles & users if they do not exist.
/// Registered as a scoped service (called once on startup).
/// </summary>
public sealed class DataSeeder
{
    private readonly PermigoDbContext _db;
    private readonly UserManager<AppUser> _users;
    private readonly RoleManager<AppRole> _roles;
    private readonly IPasswordHasher<AppUser> _hasher;
    private readonly ILogger<DataSeeder> _log;

    public DataSeeder(
        PermigoDbContext db,
        UserManager<AppUser> users,
        RoleManager<AppRole> roles,
        IPasswordHasher<AppUser> hasher,
        ILogger<DataSeeder> log)
    {
        _db = db;
        _users = users;
        _roles = roles;
        _hasher = hasher;
        _log = log;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await SeedRolesAsync(ct);
        await SeedUsersAsync();
        await SeedLeaveTypesAsync(ct);
        await SeedLeaveEntitlementsAsync(ct);
        await SeedLeaveRequestsAsync(ct);
        await SeedWorkingDaysAsync(ct);
    }
    private async Task SeedLeaveEntitlementsAsync(CancellationToken ct)
    {
        if (await _db.Set<LeaveEntitlement>().AnyAsync(ct))
        {
            return;
        }
        _db.Set<LeaveEntitlement>().AddRange(DomainSeeds.DefaultLeaveEntitlements());
        await _db.SaveChangesAsync(ct);
        _log.LogInformation("Seeded leave entitlements");
    }

    private async Task SeedWorkingDaysAsync(CancellationToken ct)
    {
        if (await _db.WorkingDays.AnyAsync(ct))
        { 
            return;
        }
        var start = new DateOnly(DomainSeeds.CurrentYear, DateTime.UtcNow.Month, 1);

        for (int i = 0; i < 1200; i++)
        {
            DateOnly current = start.AddMonths(i);           
            int year = current.Year;
            int month = current.Month;

            bool monthSeeded = await _db.WorkingDays
                .AnyAsync(w => w.Date.Year == year && w.Date.Month == month, ct);

            if (!monthSeeded)
            {
                _db.WorkingDays.AddRange(
                    DomainSeeds.WorkingDaysForMonth(year, month));
            }
        }

        await _db.SaveChangesAsync(ct);
        _log.LogInformation("Seeded working days");
    }
    private async Task SeedRolesAsync(CancellationToken ct)
    {
        foreach (AppRole? role in new[] { DomainSeeds.SuperAdminRole(), DomainSeeds.ManagerRole(), DomainSeeds.EmployeeRole() })
        {
            if (!await _roles.Roles.AnyAsync(r => r.Id == role.Id, ct))
            {
                await _roles.CreateAsync(role);
                _log.LogInformation("Created role {Role}", role.Name);
            }
        }
    }

    private async Task SeedUsersAsync()
    {
        await EnsureUserAsync(
            DomainSeeds.SuperAdminId, "superadmin", "Super", "Admin",
            "admin@permigo.local", Gender.Male, Roles.SuperAdmin);

        await EnsureUserAsync(
            DomainSeeds.Manager1Id, "manager1", "Ayşe", "Yılmaz",
            "manager1@permigo.local", Gender.Female, Roles.Manager);

        await EnsureUserAsync(
            DomainSeeds.Manager2Id, "manager2", "Mehmet", "Demir",
            "manager2@permigo.local", Gender.Male, Roles.Employee);

        var employeeInfos = new (Guid id, string user, string fn, string ln, string email, Gender gender)[]
        {
            (DomainSeeds.EmployeeIds[0], "emp1", "Elif",   "Kara",  "emp1@permigo.local", Gender.Female),
            (DomainSeeds.EmployeeIds[1], "emp2", "Ahmet",  "Öztürk","emp2@permigo.local", Gender.Male),
            (DomainSeeds.EmployeeIds[2], "emp3", "Fatma",  "Çelik", "emp3@permigo.local", Gender.Female),
            (DomainSeeds.EmployeeIds[3], "emp4", "Kemal",  "Şahin", "emp4@permigo.local", Gender.Male),
            (DomainSeeds.EmployeeIds[4], "emp5", "Zeynep", "Acar",  "emp5@permigo.local", Gender.Female),
            (DomainSeeds.EmployeeIds[5], "emp6", "Can",    "Aslan", "emp6@permigo.local", Gender.Male)
        };

        foreach ((Guid id, string user, string fn, string ln, string email, Gender gender) info in employeeInfos)
        {
            await EnsureUserAsync(info.id, info.user, info.fn, info.ln, info.email, info.gender, Roles.Employee);
        }
    }

    private async Task EnsureUserAsync(
        Guid id, string userName, string first, string last,
        string email, Gender gender, string roleName)
    {
        if (await _users.FindByIdAsync(id.ToString()) is not null)
        {
            return;
        }
        var user = new AppUser
        {
            Id = id,
            UserName = userName,
            NormalizedUserName = userName.ToUpperInvariant(),
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            EmailConfirmed = true,
            FirstName = first,
            LastName = last,
            Gender = gender,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        user.PasswordHash = _hasher.HashPassword(user, DomainSeeds.InitialPassword);

        await _users.CreateAsync(user);
        await _users.AddToRoleAsync(user, roleName);
        _log.LogInformation("Created user {Email} with role {Role}", email, roleName);
    }

    private async Task SeedLeaveTypesAsync(CancellationToken ct)
    {
        if (await _db.LeaveTypes.AnyAsync(ct))
        {
            return;
        }
        _db.LeaveTypes.AddRange(
            DomainSeeds.AnnualLeave(),
            DomainSeeds.PaternityLeave(),
            DomainSeeds.SickLeave());

        await _db.SaveChangesAsync(ct);
        _log.LogInformation("Seeded leave types");
    }

    private async Task SeedLeaveRequestsAsync(CancellationToken ct)
    {
        if (await _db.LeaveRequests.AnyAsync(ct))
        {
            return;
        }
        LeaveRequest[] requests = new[]
        {
            NewRequest(DomainSeeds.EmployeeIds[0], DomainSeeds.AnnualLeaveId,
                       new(2025,7,1), new(2025,7,5),  DomainSeeds.Manager1Id, LeaveStatus.Approved),

            NewRequest(DomainSeeds.EmployeeIds[1], DomainSeeds.SickLeaveId,
                       new(2025,8,3), new(2025,8,5),  null,                  LeaveStatus.Pending),

            NewRequest(DomainSeeds.EmployeeIds[2], DomainSeeds.PaternityLeaveId,
                       new(2025,7,15), new(2025,7,29), DomainSeeds.Manager2Id, LeaveStatus.Approved),

            NewRequest(DomainSeeds.EmployeeIds[3], DomainSeeds.SickLeaveId,
                       new(2025,8,1), new(2025,8,1),  DomainSeeds.Manager2Id, LeaveStatus.Approved),

            NewRequest(DomainSeeds.EmployeeIds[4], DomainSeeds.AnnualLeaveId,
                       new(2025,8,10), new(2025,8,12), DomainSeeds.Manager1Id, LeaveStatus.Rejected),

            NewRequest(DomainSeeds.EmployeeIds[5], DomainSeeds.AnnualLeaveId,
                       new(2025,8,20), new(2025,8,26), null,                  LeaveStatus.Pending)
        };

        _db.LeaveRequests.AddRange(requests);
        await _db.SaveChangesAsync(ct);
        _log.LogInformation("Seeded leave requests");
    }

    private static LeaveRequest NewRequest(
        Guid employeeId, Guid leaveTypeId,
        DateOnly start, DateOnly end,
        Guid? approverId, LeaveStatus status)
    {
        int days = (end.DayNumber - start.DayNumber) + 1;
        return new LeaveRequest
        {
            Id = Guid.NewGuid(),
            RequestingEmployeeId = employeeId,
            LeaveTypeId = leaveTypeId,
            StartDate = start,
            EndDate = end,
            DurationDays = days,
            ApproverId = approverId,
            Status = status,
            Reason = status == LeaveStatus.Rejected ? "Exceeded team capacity" : null,
            CreatedAt = DateTime.UtcNow,
            CreateUserId = employeeId
        };
    }
}
