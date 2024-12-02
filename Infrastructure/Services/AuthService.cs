using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;
using Microsoft.Extensions.Options;


namespace api_completa_mongodb_net_6_0.Infrastructure.Services;

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtConfig _jwtConfig;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IOptions<JwtConfig> jwtConfig)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<string> LoginAsync(LoginUserDto loginDto)
        {
            User? user = await _userRepository.GetUserByEmailAsync(loginDto.Email);

            if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.Password))
                throw new UnauthorizedAccessException("Credenciales inválidas");

            return JwtHelper.GenerateToken(_jwtConfig.SecretKey,_jwtConfig.Issuer,user, DateTime.UtcNow.AddHours(1));
           
        }

        public async Task RegisterAsync(CreateUserDto userDto)
        {
            string? hashedPassword = _passwordHasher.HashPassword(userDto.Password);

            User? newUser = new()
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = hashedPassword
            };

            await _userRepository.CreateAsync(newUser);
        }
    }

