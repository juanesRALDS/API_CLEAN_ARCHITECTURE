using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.Interfaces;

namespace api_completa_mongodb_net_6_0.Application.UseCases;

public class GeneratePasswordResetTokenUseCase
{
    private readonly IPasswordResetService _passwordResetService;

    public GeneratePasswordResetTokenUseCase(IPasswordResetService passwordResetService)
    {
        _passwordResetService = passwordResetService;
    }

    public async Task<string> ExecuteAsync(string email)
    {
        return await _passwordResetService.GenerateResetTokenAsync(email);
    }
}
