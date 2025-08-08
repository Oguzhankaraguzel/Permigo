using Application.DTOs.Employee;
using Application.DTOs.Leaves;
using Application.Features.Leaves.LeaveEntitlements.Commands.AdjustEntitlement;
using Application.Features.Leaves.LeaveEntitlements.Commands.AssignEntitlement;
using Application.Features.Leaves.LeaveEntitlements.Queries.GetEntitlement;
using Application.Features.Leaves.LeaveEntitlements.Queries.ListEntitlementsByEmployee;
using Application.Features.Leaves.LeaveTypes.Query.ListLeaveType;
using Application.Features.User.Queries.ListEmployee;
using Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Concrete;   

namespace MVCUI.Controllers;

[Authorize(Roles = $"{Roles.SuperAdmin}, {Roles.Manager}")]
public sealed class EntitlementController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    public async Task<IActionResult> Index()
    {
        Result<IReadOnlyCollection<EmployeeDto>> empRes = await _mediator.Send(new ListEmployeesQuery());

        Result<PagedResult<LeaveTypeDto>> ltRes = await _mediator.Send(new ListLeaveTypesQuery(1,1000));

        ViewBag.Employees = empRes.IsSuccess
            ? empRes.Value.Select(e => (e.Id, e.FullName)).ToList()
            : new List<(Guid, string)>();

        ViewBag.LeaveTypes = ltRes.IsSuccess
            ? ltRes.Value.Items.Select(lt => (lt.Id, lt.Name)).ToList()
            : new List<(Guid, string)>();

        return View();
    }

    public async Task<IActionResult> List(Guid employeeId, int? year = null)
    {
        Result<IReadOnlyCollection<LeaveEntitlementDto>> result = await _mediator.Send(
            new ListEntitlementsByEmployeeQuery(employeeId, year));

        if (!result.IsSuccess)
        { 
            return Problem(title: result.Error.Code, detail: result.Error.Description);
        }
        return Ok(result.Value);          
    }

    public async Task<IActionResult> Assign([FromBody] AssignEntitlementCommand cmd)
    {
        Result<Guid> result = await _mediator.Send(cmd);

        return result.IsSuccess
            ? Ok(new { id = result.Value })
            : BadRequest(new { errors = new[] { result.Error } });
    }

    public async Task<IActionResult> Adjust([FromBody] AdjustEntitlementCommand cmd)
    {
        Result result = await _mediator.Send(cmd);

        return result.IsSuccess
            ? Ok()
            : BadRequest(new { errors = new[] { result.Error } });
    }

    public async Task<IActionResult> Details(Guid id)
    {
        Result<LeaveEntitlementDto> result = await _mediator.Send(new GetEntitlementQuery(id));

        return result.IsSuccess
            ? Ok(new { dto = result.Value })
            : NotFound(result.Error);
    }
}
