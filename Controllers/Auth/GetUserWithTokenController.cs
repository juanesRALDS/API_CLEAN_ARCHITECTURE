using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;

namespace api_completa_mongodb_net_6_0.Controllers.Auth;

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
    public async Task<IActionResult> GetUserFromToken()
    {
        try
        {
            // 1. Obtener el token desde el encabezado de la solicitud
            string? authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(new { Message = "Token no proporcionado o formato incorrecto." });
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();

            // 2. Validar el token y extraer los reclamos
            ClaimsPrincipal? principal = _tokenService.ValidateTokenAndGetPrincipal(token);
            if (principal == null)
            {
                return Unauthorized(new { Message = "Token inválido o caducado." });
            }

            // 3. Obtener el ID del usuario desde los reclamos
            string? userId = principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "No se pudo identificar al usuario desde el token." });
            }

            // 4. Usar el caso de uso para obtener la información del usuario
            UserDto? user = await _getUserByTokenUseCase.Execute(userId);
            if (user == null)
            {
                return NotFound(new { Message = "Usuario no encontrado." });
            }

            // 5. Devolver la información del usuario
            return Ok(user);
        }
        catch (Exception ex)
        {
            // Manejo genérico de errores
            return StatusCode(500, new { Message = "Ocurrió un error al procesar la solicitud.", Error = ex.Message });
        }
    }
}
