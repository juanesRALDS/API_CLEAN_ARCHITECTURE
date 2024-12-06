using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.Interfaces;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Auth;

public class GeneratePasswordResetTokenUseCase
{
    private readonly IPasswordResetService _passwordResetService;

    public GeneratePasswordResetTokenUseCase(IPasswordResetService passwordResetService)
    {
        _passwordResetService = passwordResetService;
    }

    public async Task<string> Login(string email)
    {
        return await _passwordResetService.GenerateResetTokenAsync(email);
    }
}
