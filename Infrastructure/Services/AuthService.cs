using System;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Utils;
using BCrypt.Net;

namespace api_completa_mongodb_net_6_0.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Método para iniciar sesión
        public async Task<string> LoginAsync(LoginUserDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            // Verificar si el usuario existe y si la contraseña es válida
            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
                throw new UnauthorizedAccessException("Credenciales inválidas");

            // Generar un token JWT para el usuario autenticado
            return JwtHelper.GenerateToken(user);
        }

        // Método para registrar un nuevo usuario
        public async Task RegisterAsync(CreateUserDto userDto)
        {
            // Encriptar la contraseña antes de guardar al usuario
            var hashedPassword = EncryptPassword(userDto.Password);
            
            var newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = hashedPassword
            };

            // Crear el usuario en la base de datos
            await _userRepository.CreateAsync(newUser);
        }

        // Método para encriptar la contraseña
        public string EncryptPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Método para verificar la contraseña
        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
