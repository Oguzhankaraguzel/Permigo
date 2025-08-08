using Domain.Abstractions;

namespace Domain.Entities.Calender;
public class WorkingDay : BaseEntity
{
    public required DateOnly Date { get; set; }
    public bool IsWorkingDay { get; set; } = true;
}
