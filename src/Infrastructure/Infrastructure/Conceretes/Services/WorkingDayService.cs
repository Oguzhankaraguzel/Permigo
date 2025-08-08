using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Conceretes.Services;

/// <summary>
/// Provides business-level calendar utilities backed by the WorkingDays table.
/// Works with PermigoDbContext; caches results for the lifetime of the scoped service.
/// </summary>
public sealed class WorkingDayService : IWorkingDayService
{
    private readonly IApplicationDbContext _db;
    private HashSet<DateOnly>? _workingDayCache;   

    public WorkingDayService(IApplicationDbContext db) => _db = db;

    public bool IsWorkingDay(DateOnly date)
    {
        _workingDayCache ??= _db.WorkingDays.AsNoTracking()
                               .Where(w => w.IsWorkingDay)
                               .Select(w => w.Date)
                               .ToHashSet();

        return _workingDayCache.Contains(date);
    }

    public int CalculateLeaveDays(DateOnly start, DateOnly end, bool onlyWorkingDays)
    {
        if (end < start)
        { 
            throw new ArgumentException("End date cannot precede start date.", nameof(end));
        }
        IEnumerable<DateOnly> days = Enumerable.Range(0, (end.DayNumber - start.DayNumber) + 1)
                             .Select(i => start.AddDays(i));

        if (onlyWorkingDays)
        { 
            days = days.Where(IsWorkingDay);
        }
        return days.Count();
    }
}
