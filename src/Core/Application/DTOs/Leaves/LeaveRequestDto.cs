using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Leaves;
public sealed record LeaveRequestDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    Guid LeaveTypeId,
    string LeaveTypeName,
    DateOnly StartDate,
    DateOnly EndDate,
    int DurationDays,
    string Status);
