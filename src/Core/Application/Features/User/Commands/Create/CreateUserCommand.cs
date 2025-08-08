using Application.Abstractions.Messaging;

namespace Application.Features.User.Commands.Create;

public sealed record CreateUserCommand(
        string Email,
        string FirstName,
        string LastName,
        string? PhoneNumber,
        bool IsManager) : ICommand<Guid>;
