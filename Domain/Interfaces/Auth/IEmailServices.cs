namespace api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

