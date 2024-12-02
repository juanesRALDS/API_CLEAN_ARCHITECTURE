using api_completa_mongodb_net_6_0.Domain.Interfaces;

namespace api_completa_mongodb_net_6_0.Application.UseCases;

public class UpdatePasswordUseCase
{
    private readonly IPasswordResetTokenRepository _tokenRepository;

    public UpdatePasswordUseCase(IPasswordResetTokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }

    public async Task<bool> ExecuteAsync(string token, string newPassword)
    {
        // Busca el token en la base de datos
        var storedToken = await _tokenRepository.GetByTokenAsync(token);

        // Valida el token
        if (storedToken == null || storedToken.Expiration < DateTime.UtcNow)
            return false;

        // Lógica para actualizar la contraseña (implementación pendiente)
        // ...

        // Borra el token después de su uso
        await _tokenRepository.DeleteTokenAsync(token);

        return true;
    }
}
