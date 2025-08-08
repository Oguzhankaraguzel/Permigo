using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Domain.Entities.Leaves;
using Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Leaves.LeaveRequests.Commands.CreateLeaveRequest;
internal sealed class SendLeaveRequestCreatedMailHandler(
        IMailService mail,
        UserManager<AppUser> userManager)
    : INotificationHandler<LeaveRequestCreatedEvent>
{
    public async Task Handle(LeaveRequestCreatedEvent ev, CancellationToken cancellationToken)
    {
        LeaveRequest req = ev.LeaveRequest;
        IEnumerable<AppUser> managers = await userManager.GetUsersInRoleAsync(Roles.Manager)
                       ?? Enumerable.Empty<AppUser>();

        string[] managerEmails = managers
            .Select(u => u.Email)
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .ToArray()!;
        if (managerEmails.Length == 0)
        {
            return;
        }

        string subject = $"New leave request: {req.RequestingEmployee!.FullName}";
        string body = $"""
            <p>Hello,</p>
            <p><b>{req.RequestingEmployee.FullName}</b> submitted a <b>{req.LeaveType!.Name}</b> request:</p>
            <ul>
              <li><b>Dates:</b> {req.StartDate:dd.MM.yyyy} – {req.EndDate:dd.MM.yyyy}</li>
              <li><b>Total days:</b> {req.DurationDays}</li>
            </ul>
            <p>Please review it in the portal.</p>
            """;

        await mail.SendMailAsync(managerEmails, subject, body);
    }
}
