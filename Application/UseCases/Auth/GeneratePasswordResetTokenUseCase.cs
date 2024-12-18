using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces.Auth;
using SagaAserhi.Application.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Domain.Interfaces.Auth;
using SagaAserhi.Infrastructure.Config;
using SagaAserhi.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using SagaAserhi.Application.Interfaces;

namespace SagaAserhi.Application.UseCases.Auth;

public class GeneratePasswordResetTokenUseCase : IGeneratePasswordResetTokenUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly JwtConfig _jwtConfig;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GeneratePasswordResetTokenUseCase(
        IUserRepository userRepository,
        IPasswordResetTokenRepository tokenRepository,
        IEmailService emailService,
        IOptions<JwtConfig> jwtconfig,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _emailService = emailService;
        _jwtConfig = jwtconfig.Value;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<string> Execute(string email)
    {
        User? user = await _userRepository.GetUserByEmail(email)
            ?? throw new Exception("Usuario no encontrado con el correo proporcionado.");

        DateTime expiration = DateTime.UtcNow.AddMinutes(30);
        string tokenValue = JwtHelper.GenerateToken(
            jwtConfig: _jwtConfig,
            user: user,
            expiration: expiration
        );

        Token? token = new()
        {
            Tokens = tokenValue,
            Expiration = expiration,
            UserId = user.Id
        };

        HttpContext? httpContext = _httpContextAccessor.HttpContext
          ?? throw new InvalidOperationException("HttpContext no disponible");

        string scheme = httpContext.Request.Scheme ?? "https";
        string host = httpContext.Request.Host.Value ?? "localhost";
        string callbackUrl = $"{scheme}://{host}/reset-password?token={tokenValue}";

        string subject = "Restablecimiento de contraseña";
        string body = $@"
            <h1>Solicitud de Restablecimiento de Contraseña</h1>
            <p>Hola {user.Name},</p>
            <p>Recibimos una solicitud para restablecer tu contraseña. Puedes hacerlo haciendo clic en el siguiente enlace:</p>
            <a href='{callbackUrl}'>Restablecer Contraseña</a>
            <p>Si no realizaste esta solicitud, ignora este correo.</p>
            <p>Este enlace expirará en 30 minutos.</p>";

        try
        {
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
        catch (System.Exception ex)
        {

            throw new InvalidOperationException($"Error al enviar el correo {ex.Message}", ex);
        }

        await _tokenRepository.SaveToken(token);

        return "El enlace para restablecer la contraseña ha sido enviado a tu correo electrónico.";
    }
}
