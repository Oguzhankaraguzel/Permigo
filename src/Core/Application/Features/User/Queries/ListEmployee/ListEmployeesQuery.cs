using Application.Abstractions.Messaging;
using Application.DTOs.Employee;

namespace Application.Features.User.Queries.ListEmployee;

public sealed record ListEmployeesQuery()
    : IQuery<IReadOnlyCollection<EmployeeDto>>;
