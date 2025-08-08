using Application.Abstractions.Services;
using Domain.Entities.Leaves;
using MediatR;

namespace Application.Features.Leaves.LeaveRequests.Commands.CreateLeaveRequest;

internal sealed class SendLeaveRequestRejectedMailHandler(IMailService mail)
    : INotificationHandler<LeaveRequestRejectedEvent>
{
    public async Task Handle(LeaveRequestRejectedEvent ev, CancellationToken cancellationToken)
    {
        LeaveRequest req = ev.LeaveRequest;
        string? email = req.RequestingEmployee?.Email;
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        string subject = "Your leave request was rejected";
        string body = $"""
            <p>Dear {req.RequestingEmployee!.FullName},</p>
            <p>Your <b>{req.LeaveType!.Name}</b> request for
            {req.StartDate:dd.MM.yyyy} – {req.EndDate:dd.MM.yyyy} ({req.DurationDays} days)
            has been <b>rejected</b>.</p>
            {(!string.IsNullOrWhiteSpace(req.Reason) ? $"<p><b>Reason:</b> {req.Reason}</p>" : "")}
            <p>Please contact your manager or HR for more information.</p>
            """;

        await mail.SendMailAsync(new[] { email }, subject, body);
    }
}
