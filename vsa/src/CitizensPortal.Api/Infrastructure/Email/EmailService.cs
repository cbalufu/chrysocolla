namespace CitizensPortal.Api.Infrastructure.Email;

public sealed class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string to, string subject, string body, string priority = "Medium")
    {
        // For now, just log the email. In production, this would send via SMTP
        _logger.LogInformation(
            "Sending email to {To} with subject '{Subject}' and priority '{Priority}'",
            to, subject, priority);
        
        _logger.LogDebug("Email body: {Body}", body);

        // In production, implement actual email sending using MailKit or similar:
        // using var client = new SmtpClient();
        // await client.ConnectAsync(_smtpHost, _smtpPort, _useSsl);
        // await client.AuthenticateAsync(_smtpUser, _smtpPassword);
        // var message = new MimeMessage { ... };
        // await client.SendAsync(message);
        // await client.DisconnectAsync(true);

        return Task.CompletedTask;
    }
}
