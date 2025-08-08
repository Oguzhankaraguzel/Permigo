using Application.Abstractions.Messaging;
using Application.DTOs.Employee;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Concrete;

namespace Application.Features.User.Queries.ListEmployee;
internal sealed class ListEmployeesHandler(UserManager<AppUser> userManager)
    : IQueryHandler<ListEmployeesQuery, IReadOnlyCollection<EmployeeDto>>
{
    public async Task<Result<IReadOnlyCollection<EmployeeDto>>> Handle(ListEmployeesQuery request, CancellationToken cancellationToken)
    {
        IList<AppUser> employees = await userManager.GetUsersInRoleAsync(Roles.Employee);

        return employees
            .Select(u => new EmployeeDto(u.Id, u.FullName))
            .OrderBy(e => e.FullName)
            .ToList();
    }
}
