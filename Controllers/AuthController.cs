using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace api_completa_mongodb_net_6_0.Controllers

{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionServices _encryptionService;

        public AuthController(IUserRepository userRepository, IEncryptionServices encryptionService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Encriptar la contraseña antes de guardarla en la base de datos.
            user.Password = _encryptionService.EncryptPassword(user.Password);
            await _userRepository.CreateAsync(user);
            return Ok("User created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            // Verificar si el usuario existe por su email.
            var user = await _userRepository.GetByEmailAsync(loginUser.Email);
            if (user == null || !_encryptionService.VerifyPassword(loginUser.Password, user.Password))
                return Unauthorized("Invalid credentials");

            return Ok("Login successful");
        }
    }
}
