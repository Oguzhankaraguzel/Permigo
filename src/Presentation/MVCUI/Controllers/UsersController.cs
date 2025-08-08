using Application.Features.User.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCUI.Extensions;
using SharedKernel.Concrete;

namespace MVCUI.Controllers;

[Authorize(Roles = "SuperAdmin, Manager")]
public class UsersController(ISender sender) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        Result<Guid> result = await sender.Send(command);

        return result.Match<Guid, IActionResult>(
        id => Ok(new
        {
            userId = id,
            message = "User created successfully."
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
