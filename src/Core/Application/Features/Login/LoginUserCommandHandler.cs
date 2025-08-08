using Application.Abstractions.Authentications;
using Application.Abstractions.Messaging;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Concrete;

namespace Application.Features.Login;

internal sealed class LoginUserCommandHandler(ITokenProvider tokenProvider,
                                              UserManager<AppUser> userManager) 
    : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        bool looksLikeEmail = request.Identifier.Contains('@');

        AppUser? user = looksLikeEmail
            ? await userManager.FindByEmailAsync(request.Identifier)
            : await userManager.FindByNameAsync(request.Identifier);

        if (user is null)
        {
            return Result.Failure<string>(UserError.NotFoundByUserName);
        }

        bool verified = await userManager.CheckPasswordAsync(user!, request.Password);

        if (!verified)
        {
            return Result.Failure<string>(UserError.InvalidCredentials);
        }

        string token = tokenProvider.GenerateToken(user,await userManager.GetRolesAsync(user));
        return token;
    }
}
