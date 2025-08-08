using Application.DTOs.Leaves;
using Application.Features.Leaves.LeaveTypes.Command.Create;
using Application.Features.Leaves.LeaveTypes.Query.GetLeaveType;
using Application.Features.Leaves.LeaveTypes.Query.ListLeaveType;
using Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCUI.Extensions;
using SharedKernel.Concrete;

namespace MVCUI.Controllers;
[Authorize(Roles = $"{Roles.SuperAdmin}, {Roles.Manager}")]
public sealed class LeaveTypeController(ISender sender) : Controller
{
    public async Task<IActionResult> Index()
    {
        Result<PagedResult<LeaveTypeDto>> result = await sender.Send(new ListLeaveTypesQuery());

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(result.Error.Code, result.Error.Description);
            return View(new PagedResult<LeaveTypeDto>(Array.Empty<LeaveTypeDto>(), 1, 10, 0));
        }

        return View(result.Value);
    }
    [HttpGet]
    public async Task<IActionResult> List(int page = 1, int pageSize = 10)
    {
        Result<PagedResult<LeaveTypeDto>> result = await sender.Send(new ListLeaveTypesQuery(page, pageSize));
        return result.Match(
            ok => Ok(new { items = ok.Items, page = ok.Page, totalPages = ok.TotalPages }),
            err => Problem(err.Error.Code, err.Error.Description));
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLeaveTypeCommand cmd)
    {
        try
        {
            Result<Guid> result = await sender.Send(cmd);
            return result.Match<Guid, IActionResult>(
        id => Ok(new
        {
            id = id,
            message = "Leave Type created successfully."
        }),
        failure =>
        {
            if (failure.Error is ValidationError validationError)
            {
                return BadRequest(new { errors = validationError.Errors });
            }
            return BadRequest(new { errors = new[] { failure.Error } });
        });
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
        
    }

    public async Task<IActionResult> Details(Guid id)
    {
        Result<LeaveTypeDto>? result = await sender.Send(new GetLeaveTypeQuery(id));
        return result.Match<LeaveTypeDto, IActionResult>(
        dto => Ok(new
        {
            dto
        }),
        failure =>
        {
            if (failure.Error is ValidationError validationError)
            {
                return BadRequest(new { errors = validationError.Errors });
            }
            return BadRequest(new { errors = new[] { failure.Error } });
        });
    }
}
