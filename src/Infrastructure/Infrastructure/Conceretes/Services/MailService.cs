using System.Net;
using System.Net.Mail;
using Application.Abstractions.Services;
using Application.DTOs;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Conceretes.Services;

public sealed class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMailAsync(
        string[] tos,
        string subject,
        string body,
        bool isBodyHtml = true,
        IEnumerable<MailAttachment>? attachments = null)
    {
        if (!string.Equals(_configuration["Mail:IsTheServiceActive"], "true", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        using var mailMessage = new MailMessage
        {
            IsBodyHtml = isBodyHtml,
            Subject = subject,
            Body = body,
            From = new MailAddress(
                _configuration["Mail:Username"]!,
                _configuration["Mail:FromName"]!) 
        };

        foreach (string to in tos)
        {
            mailMessage.To.Add(to);
        }

        if (attachments is not null)
        {
            foreach (MailAttachment attachment in attachments)
            {
                attachment.Content.Position = 0;
                mailMessage.Attachments.Add(new Attachment(attachment.Content, attachment.FileName, attachment.ContentType));
            }
        }

        int port = int.TryParse(_configuration["Mail:Port"], out int p) ? p : 587;
        bool enableSsl = !string.Equals(_configuration["Mail:Ssl"], "false", StringComparison.OrdinalIgnoreCase);

        using var smtpClient = new SmtpClient
        {
            Host = _configuration["Mail:Host"]!,
            Port = port,
            EnableSsl = enableSsl,
            Credentials = new NetworkCredential(
                _configuration["Mail:Username"],
                _configuration["Mail:Password"])
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
}
