using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;
using Microsoft.IdentityModel.Tokens;
using ZstdSharp;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Auth;

public class UpdatePasswordUseCase
{
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;

    public UpdatePasswordUseCase(IPasswordResetTokenRepository tokenRepository, IUserRepository userRepository)
    {
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> Login(string token, string newPassword)
    {
        
        Domain.Entities.Token? storedToken = await _tokenRepository.GetByTokenAsync(token);

        if (storedToken == null)
        {
            throw new InvalidOperationException("el Token no se encontro o es nulo");

        }if(storedToken.Expiration.Date < DateTime.UtcNow.Date)
        {
            throw new SecurityTokenExpiredException($"el token ha expirado, Expiracion{storedToken.Expiration}, tiempo actual: {DateTime.UtcNow}");
        }


        Domain.Entities.User? user = await _userRepository.GetByIdAsync(storedToken.UserId) 
            ?? throw new KeyNotFoundException("el usuario asociado al token no se encontro");


        string? hashedPassword = HashingHelper.HashPassword(newPassword);

        
        await _userRepository.UpdatePasswordAsync(user.Id, hashedPassword);


        await _tokenRepository.DeleteTokenAsync(token);

        return true;
    }
}
