using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;
using Microsoft.Extensions.Options;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Auth;

public class GeneratePasswordResetTokenUseCase : IGeneratePasswordResetTokenUseCase
{

    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly JwtConfig _jwtConfig;
    private readonly IEmailService _emailService;
    public GeneratePasswordResetTokenUseCase(IUserRepository userRepository, IPasswordResetTokenRepository tokenRepository, JwtConfig jwtConfig, IEmailService emailService,IOptions<JwtConfig> jwtconfig)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _emailService = emailService;
        _jwtConfig = jwtconfig.Value;
    }

    public async Task<string> Execute(string email)
    {

        User? user = await _userRepository.GetUserByEmailAsync(email)
            ?? throw new Exception("Usuario no encontrado con el correo proporcionado.");

        // Generar el token usando JwtHelper
        DateTime expiration = DateTime.UtcNow.AddMinutes(30);
        string? tokenValue = JwtHelper.GenerateToken(
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

        string resetUrl = $"https://tu-dominio.com/reset-password?token={tokenValue}";
        string subject = "Restablecimiento de contraseña";
        string body = $@"
            <h1>Solicitud de Restablecimiento de Contraseña</h1>
            <p>Hola {user.Name},</p>
            <p>Recibimos una solicitud para restablecer tu contraseña. Puedes hacerlo haciendo clic en el siguiente enlace:</p>
            <a href='{resetUrl}'>Restablecer Contraseña</a>
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

        await _tokenRepository.SaveTokenAsync(token);

        return "El enlace para restablecer la contraseña ha sido enviado a tu correo electrónico.";
    }
}
