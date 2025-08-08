using Application.DTOs.Leaves;
using Application.Features.Leaves.LeaveRequests.Commands.ApproveLeaveRequest;
using Application.Features.Leaves.LeaveRequests.Commands.CreateLeaveRequest;
using Application.Features.Leaves.LeaveRequests.Commands.RejectLeaveRequest;
using Application.Features.Leaves.LeaveRequests.Queries.ListLeaveRequests;
using Application.Features.Leaves.LeaveTypes.Query.ListLeaveType;
using Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCUI.Extensions;
using SharedKernel.Concrete;

namespace MVCUI.Controllers;

[Authorize]
public sealed class LeaveRequestController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]                       
    public IActionResult Index() => View();

    public async Task<IActionResult> List(string status = "Pending")
    {
        Result<IReadOnlyCollection<LeaveRequestDto>> result = await _mediator.Send(new ListLeaveRequestsQuery(status));

        if (!result.IsSuccess)
        { 
            return Problem(title: result.Error.Code, detail: result.Error.Description);
        }
        return Ok(result.Value);   
    }

    [Authorize(Roles = $"{Roles.SuperAdmin}, {Roles.Manager}")]
    public async Task<IActionResult> Approve([FromBody] ApproveLeaveRequestCommand cmd)
    {
        try
        {
            Result res = await _mediator.Send(cmd);

            return res.Match<IActionResult>(
                () => Ok(new { message = "Leave request approved successfully." }),

                failure =>
                {
                    if (failure.Error is ValidationError validationError)
                    {
                        return BadRequest(new { errors = validationError.Errors });
                    }
                    return BadRequest(new { errors = new[] { failure.Error } });
                });
        }
        catch (Exception)
        {
            return StatusCode(500, new { errors = new[] { new { Code = "InternalServerError", Description = "An unexpected error occurred." } } });
        }
    }

    [Authorize(Roles = $"{Roles.SuperAdmin}, {Roles.Manager}")]
    public async Task<IActionResult> Reject([FromBody] RejectLeaveRequestCommand cmd)
    {
        try
        {
            Result res = await _mediator.Send(cmd);

            return res.Match<IActionResult>(
                () => Ok(new { message = "Leave request rejected successfully." }),
                failure =>
                {
                    if (failure.Error is ValidationError validationError)
                    {
                        return BadRequest(new { errors = validationError.Errors });
                    }
                    return BadRequest(new { errors = new[] { failure.Error } });
                });
        }
        catch (Exception)
        {
            return StatusCode(500, new { errors = new[] { new { Code = "InternalServerError", Description = "An unexpected error occurred." } } });
        }
    }
    [Authorize(Roles = Roles.Employee)]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        // basit bir drop-down için izin türlerini getir
        Result<PagedResult<LeaveTypeDto>> res = await _mediator.Send(new ListLeaveTypesQuery(1, int.MaxValue));
        if (!res.IsSuccess)
        {
            return Problem(res.Error.Code, res.Error.Description);
        }

        ViewBag.LeaveTypes = res.Value.Items;
        return View();
    }

    [Authorize(Roles = Roles.Employee)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLeaveRequestCommand cmd)
    {
        Result<Guid> res = await _mediator.Send(cmd);

        return res.Match<IActionResult>(
            () => Ok(new { message = "Leave request created.", requestId = res.Value }),
            failure =>
            {
                if (failure.Error is ValidationError ve)
                {
                    return BadRequest(new { errors = ve.Errors });
                }

                return BadRequest(new { errors = new[] { failure.Error } });
            });
    }
}
