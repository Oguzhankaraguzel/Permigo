namespace MVCUI.Models.Request;

public sealed record LoginUserRequest
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
}
