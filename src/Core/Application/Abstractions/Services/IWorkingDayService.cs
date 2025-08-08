namespace Application.Abstractions.Services;

public interface IWorkingDayService
{
    /// <summary>Returns the total number of leave days between two dates.
    /// If <paramref name="onlyWorkingDays"/> is true, weekends/public holidays excluded</summary>
    int CalculateLeaveDays(DateOnly start, DateOnly end, bool onlyWorkingDays);

    /// <summary>Indicates whether a particular date is marked as a working day in the database.</summary>
    bool IsWorkingDay(DateOnly date);
}
