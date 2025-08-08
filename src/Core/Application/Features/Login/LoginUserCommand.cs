using Application.Abstractions.Messaging;

namespace Application.Features.Login;

public sealed record LoginUserCommand : ICommand<string>
{
    public string Identifier { get; set; }
    public string Password { get; set; }
}
