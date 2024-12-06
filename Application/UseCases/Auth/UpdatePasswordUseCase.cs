using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;

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
        // Busca el token en la base de datos
        Domain.Entities.Token? storedToken = await _tokenRepository.GetByTokenAsync(token);
        Console.WriteLine($"Expiración del token almacenado: {storedToken.Expiration}");
        Console.WriteLine($"Fecha actual: {DateTime.UtcNow}");

        // Valida el token
        if (storedToken == null || storedToken.Expiration.Date < DateTime.UtcNow.Date)
        {
            Console.WriteLine("Token expirado.");
            Console.WriteLine($"Token inválido o expirado. Expiración: {storedToken?.Expiration}, Ahora: {DateTime.UtcNow}");
            return false;
        }

        // Lógica para actualizar la contraseña (implementación pendiente)
        Domain.Entities.User? user = await _userRepository.GetByIdAsync(storedToken.UserId);

        if (user == null)
        {
            Console.WriteLine("usuario no encontrado");
            return false;

        }

        //encriptacion de contraseña
        string? hashedPassword = HashingHelper.HashPassword(newPassword);

        //actualizar contraseña del usuario
        await _userRepository.UpdatePasswordAsync(user.Id, hashedPassword);

        // Borra el token después de su uso
        await _tokenRepository.DeleteTokenAsync(token);

        return true;
    }
}
