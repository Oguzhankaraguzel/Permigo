using Application.Abstractions.Services;
using Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.Features.User.Commands.Create;
internal class UserRegisteredDomainEventHandler(UserManager<AppUser> _userManager,
                                                IConfiguration _configuration, 
                                                IMailService _mailService) 
    : INotificationHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        AppUser? user = await _userManager.FindByIdAsync(notification.UserId.ToString());
        
        if (user is not null)
        {
            string roleName = await _userManager.IsInRoleAsync(user, Roles.Manager) ? Roles.Manager : Roles.Employee;
            string appUrl = _configuration["App:BaseUrl"]!;
            string link = $"{appUrl}/account/setpassword?userId={user.Id}";

            await _mailService.SendMailAsync([user.Email!],
                                             "Your Account has been Created | Permigo",
                                                $"<p>Hello {user.FirstName!},</p><p>Your Permigo {roleName} account has been created. Click the link below to set your password:</p><p><a href=\"{link}\">Create My Password</a></p><p>Thank you.</p>",
                                                true
                                             );
        }
    }
}
