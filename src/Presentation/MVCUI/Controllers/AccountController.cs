using Application.Features.Login;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MVCUI.Extensions;
using MVCUI.Models.Request;
using SharedKernel.Concrete;

namespace MVCUI.Controllers;

public class AccountController : Controller
{
    private readonly ISender _sender;

    public AccountController(ISender sender)
    {
        _sender = sender;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        Result<string> result = await _sender.Send(new LoginUserCommand
        {
            Password = request.Password,
            Identifier = request.UserNameOrEmail
        });

        return result.Match<string, IActionResult>(
        successValue =>
        {
            HttpContext.Session.SetString("Token", successValue);
            return RedirectToAction("Index", "Home");  
        },
        failure =>
        {
            ModelState.AddModelError(string.Empty, failure.Error.Description);
            return View("Index", request);              
        });
    }
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
    public IActionResult SetPassword()
    {
        return View();
    }
}
