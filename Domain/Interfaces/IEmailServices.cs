

namespace api_completa_mongodb_net_6_0.MongoApiDemo.Infrastructure.Interfaces;
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

