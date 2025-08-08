using Application.DTOs;

namespace Application.Abstractions.Services;

public interface IMailService
{
    Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true, IEnumerable<MailAttachment>? attachments = null);
}
