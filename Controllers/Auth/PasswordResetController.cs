
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using Microsoft.AspNetCore.Mvc;

namespace api_completa_mongodb_net_6_0.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class PasswordResetController : ControllerBase
{
    private readonly GeneratePasswordResetTokenUseCase _GeneratePasswordReset;

    public PasswordResetController(GeneratePasswordResetTokenUseCase useCase)
    {
        _GeneratePasswordReset = useCase;
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] TokenRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return BadRequest(new { Message = "El correo electrónico es obligatorio." });

        try
        {
            string? resetToken = await _GeneratePasswordReset.Execute(request.Email);
            if (string.IsNullOrEmpty(resetToken))
            {
                return BadRequest(new {Message = "no se pudo generar el token de restablecimiento"});
            }

            string resetUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/reset-password?token={resetToken}";

            Console.WriteLine(resetUrl);

            return Ok( new 
            {
                Message = "Token generado con éxito.",
                ResetUrl = resetUrl
            }
            );

        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}
