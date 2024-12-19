using System.Net;
using System.Net.Mail;
using SagaAserhi.Application.Interfaces.Auth;

namespace SagaAserhi.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPassword;

    public EmailService(IConfiguration configuration)
    {
        IConfigurationSection? emailSettings = configuration.GetSection("EmailSettings");
        _smtpServer = emailSettings["SmtpServer"];
        _smtpPort = int.Parse(emailSettings["SmtpPort"]);
        _smtpUser = emailSettings["SmtpUser"];
        _smtpPassword = emailSettings["SmtpPassword"];
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            using SmtpClient? client = new(_smtpServer, _smtpPort);
            client.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
            client.EnableSsl = true;

            MailMessage? mailMessage = new()
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error sending email: {ex.Message}", ex);
        }
    }
}

