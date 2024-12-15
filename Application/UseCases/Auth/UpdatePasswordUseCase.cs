using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Utils;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;
using Microsoft.IdentityModel.Tokens;
using ZstdSharp;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Auth;

public class UpdatePasswordUseCase : IUpdatePasswordUseCase
{
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _PasswordHelper;

    public UpdatePasswordUseCase(
        IPasswordResetTokenRepository tokenRepository, 
        IUserRepository userRepository, 
        IPasswordHasher passwordHelper
    )
    {
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
        _PasswordHelper = passwordHelper;
    }

    public async Task<bool> Execute(string token, string newPassword)
    {
        
        Token? storedToken = await _tokenRepository.GetByToken(token);

        if (storedToken == null)
        {
            throw new InvalidOperationException("el Token no se encontro o es nulo");

        }if(storedToken.Expiration.Date < DateTime.UtcNow.Date)
        {
            throw new SecurityTokenExpiredException(
            $"el token ha expirado, Expiracion{storedToken.Expiration}, tiempo actual: {DateTime.UtcNow}"
            );
        }


        User? user = await _userRepository.GetUserById(storedToken.UserId) 
            ?? throw new KeyNotFoundException("el usuario asociado al token no se encontro");


        string? hashedPassword = _PasswordHelper.HashPassword(newPassword);

        
        await _userRepository.UpdatePassword(user.Id, hashedPassword);


        await _tokenRepository.DeleteToken(token);

        return true;
    }
}
