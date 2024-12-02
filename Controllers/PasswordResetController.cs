using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace api_completa_mongodb_net_6_0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PasswordResetController : ControllerBase
{
    private readonly GeneratePasswordResetTokenUseCase _useCase;

    public PasswordResetController(GeneratePasswordResetTokenUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] TokenRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return BadRequest(new { Message = "El correo electrónico es obligatorio." });

        try
        {
            var resetUrl = await _useCase.ExecuteAsync(request.Email);
            return Ok(new { Message = "Token generado con éxito.", Url = resetUrl });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
