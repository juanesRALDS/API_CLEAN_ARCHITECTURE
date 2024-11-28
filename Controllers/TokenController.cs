using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api_completa_mongodb_net_6_0.Application.UseCases;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using System.Security.Claims;

namespace api_completa_mongodb_net_6_0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly GetUserByTokenUseCase _getUserByTokenUseCase;

        public TokenController(ITokenService tokenService, GetUserByTokenUseCase getUserByTokenUseCase)
        {
            _tokenService = tokenService;
            _getUserByTokenUseCase = getUserByTokenUseCase;
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUserFromToken()
        {
            // Obtiene el token del encabezado
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token)) 
                return BadRequest("Token no proporcionado.");

            // Valida el token y extrae los reclamos
            var principal = _tokenService.ValidateTokenAndGetPrincipal(token);
            if (principal == null) 
                return Unauthorized("Token inválido o caducado.");

            // Obtiene la información del usuario directamente desde los Claims
            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (userId == null) 
                return Unauthorized("No se pudo identificar al usuario.");

            // Llama al caso de uso para obtener el usuario
            var user = _getUserByTokenUseCase.ExecuteAsync(userId).Result;
            if (user == null) 
                return NotFound("Usuario no encontrado.");

                
            return Ok(new 
            { 
                UserId = userId, 
                Email = email, 
                Nombre = user.Name,
            });
        }
    }
}
