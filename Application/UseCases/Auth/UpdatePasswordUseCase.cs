using SagaAserhi.Application.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Domain.Interfaces.Auth;
using SagaAserhi.Domain.Interfaces.Utils;
using SagaAserhi.Infrastructure.Utils;
using Microsoft.IdentityModel.Tokens;
using ZstdSharp;

namespace SagaAserhi.Application.UseCases.Auth;

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
            throw new InvalidOperationException("Token not found or is null");
        }

        if (storedToken.Expiration < DateTime.UtcNow)
        {
            throw new SecurityTokenExpiredException(
                $"Token has expired, expiration: {storedToken.Expiration}, current time: {DateTime.UtcNow}"
            );
        }

        User? user = await _userRepository.GetUserById(storedToken.UserId) 
            ?? throw new KeyNotFoundException("The user associated with the token was not found");

        string hashedPassword = _PasswordHelper.HashPassword(newPassword);
        await _userRepository.UpdatePassword(user.Id, hashedPassword);
        await _tokenRepository.DeleteToken(token);

        return true;
    }
}
