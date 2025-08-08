using Application.Abstractions.Services;
using Domain.Entities.Leaves;
using MediatR;

namespace Application.Features.Leaves.LeaveRequests.Commands.CreateLeaveRequest;

internal sealed class SendLeaveRequestApprovedMailHandler(IMailService mail)
    : INotificationHandler<LeaveRequestApprovedEvent>
{
    public async Task Handle(LeaveRequestApprovedEvent ev, CancellationToken cancellationToken)
    {
        LeaveRequest req = ev.LeaveRequest;
        string? email = req.RequestingEmployee?.Email;
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        string subject = "Your leave request is approved";
        string body = $"""
            <p>Dear {req.RequestingEmployee!.FullName},</p>
            <p>Your <b>{req.LeaveType!.Name}</b> request for
            {req.StartDate:dd.MM.yyyy} – {req.EndDate:dd.MM.yyyy} ({req.DurationDays} days)
            has been <b>approved</b>.</p>
            <p>Enjoy your time off!</p>
            """;

        await mail.SendMailAsync(new[] { email }, subject, body);
    }
}
