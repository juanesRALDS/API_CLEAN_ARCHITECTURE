using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;

namespace api_completa_mongodb_net_6_0.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly IGetUserByTokenUseCase _getUserByTokenUseCase;

    public TokenController(
        IGetUserByTokenUseCase getUserByTokenUseCase
        )
    {
        _getUserByTokenUseCase = getUserByTokenUseCase;
    }


    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> GetUserFromToken()
    {
        try
        {
            string? authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(new { Message = "Token no proporcionado o formato incorrecto." });
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();

            UserDto? user = await _getUserByTokenUseCase.Execute(token);
            if (user == null)
            {
                return Unauthorized(new { Message = "Token inválido o caducado o usuario no encontrado." });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocurrió un error al procesar la solicitud.", Error = ex.Message });
        }
    }
}
