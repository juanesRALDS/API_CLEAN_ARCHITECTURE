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
            var tokens = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(tokens)) 
                return BadRequest("Token no proporcionado.");

            // Valida el token y extrae los reclamos
            var principal = _tokenService.ValidateTokenAndGetPrincipal(tokens);
            if (principal == null) 
                return Unauthorized("Token inválido o caducado.");

            // Obtiene la información del usuario directamente desde los Claims
            var userId = principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;


            if (userId == null) 
                return Unauthorized("No se pudo identificar al usuario.");

            // Llama al caso de uso para obtener el usuario
            var user = _getUserByTokenUseCase.ExecuteAsync(userId).Result;
            if (user == null) 
                return NotFound("Usuario no encontrado.");

                
            return Ok(user);
        }
    }
}
