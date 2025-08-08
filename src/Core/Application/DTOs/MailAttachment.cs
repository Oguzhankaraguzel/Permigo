namespace Application.DTOs;

public sealed record MailAttachment
{
    /// <summary>
    /// Dosyanın içeriğini tutan akış (stream).
    /// </summary>
    public Stream Content { get; set; }

    /// <summary>
    /// E-postada görünecek dosya adı (örn: "rapor.pdf").
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Dosyanın MIME türü (örn: "application/pdf", "image/jpeg").
    /// </summary>
    public string ContentType { get; set; }

    public MailAttachment(Stream content, string fileName, string contentType)
    {
        Content = content;
        FileName = fileName;
        ContentType = contentType;
    }
}
