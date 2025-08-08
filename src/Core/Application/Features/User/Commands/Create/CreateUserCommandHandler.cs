using Application.Abstractions.Services;
using Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Concrete;

namespace Application.Features.User.Commands.Create;
public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IPublisher _publisher;
    private readonly ITextUtilityService _utilityService;

    public CreateUserCommandHandler(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IPublisher publisher,
        ITextUtilityService utilityService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _publisher = publisher;
        _utilityService = utilityService;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            AppUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                return Result.Failure<Guid>(UserError.EmailNotUnique);
            }
            string roleName = request.IsManager ? Roles.Manager : Roles.Employee;
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new AppRole() { Name = roleName });
            }
            user = new AppUser
            {
                UserName = _utilityService.CreateUserName(request.FirstName + "." + request.LastName),
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true,
            };
            /*
             * Geliştirme ortamında olduğu için şifre sabit verildi.
             * Prod ortamında bu şekilde sabit şifre verilmemeli.
             * Eğer mail servisi aktifleştirilise, 
             * kullanıcıya şifresini oluşturabileceği link'in olduğu bir mail gönderilmektedir.
             */
            IdentityResult createResult = await _userManager.CreateAsync(user, "P@ssword123");
            if (!createResult.Succeeded)
            {
                return Result.Failure<Guid>(Error.Failure("Users.ErrorCreatingUser",
                                                          string.Join("-", createResult.Errors.Select(x => x.Description))
                                                          )
                    );
            }
            await _publisher.Publish(new UserRegisteredDomainEvent(user.Id), cancellationToken);
            await _userManager.AddToRoleAsync(user, roleName);
            return user.Id;
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(Error.Failure("Users.ErrorCreatingUser", ex.Message));
        }
    }
}
