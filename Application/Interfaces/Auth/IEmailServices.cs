namespace SagaAserhi.Application.Interfaces.Auth;
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

