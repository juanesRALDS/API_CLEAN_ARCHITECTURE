using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using Microsoft.AspNetCore.Mvc;

namespace api_completa_mongodb_net_6_0.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class PasswordController : ControllerBase
{
    private readonly UpdatePasswordUseCase _updatePasswordUseCase;

    public PasswordController(UpdatePasswordUseCase updatePasswordUseCase)
    {
        _updatePasswordUseCase = updatePasswordUseCase;
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
    {

        if (string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.Tokens))
            return BadRequest("la nueva contraseña y el token son requeridas");

        bool result = await _updatePasswordUseCase.Login(request.Tokens, request.NewPassword);

        if (!result)
            return BadRequest("El token es inválido o ha expirado.");

        return Ok("Contraseña actualizada con éxito.");
    }
}

