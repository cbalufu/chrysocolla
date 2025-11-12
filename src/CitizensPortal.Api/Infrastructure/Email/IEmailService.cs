namespace CitizensPortal.Api.Infrastructure.Email;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, string priority = "Medium");
}
