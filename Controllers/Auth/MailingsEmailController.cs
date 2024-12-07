using Microsoft.AspNetCore.Mvc;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;

namespace api_completa_mongodb_net_6_0.Controllers.Auth;
[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    // El servicio es inyectado automáticamente
    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send-test-email")]
    public async Task<IActionResult> SendTestEmail([FromBody] string recipientEmail)
    {
        if (string.IsNullOrWhiteSpace(recipientEmail))
        {
            return BadRequest("La dirección de correo es obligatoria.");
        }

        try
        {
            await _emailService.SendEmailAsync(
                recipientEmail,
                "Correo de prueba",
                "<h1>¡Hola!</h1><p>Este es un correo de prueba enviado desde tu aplicación.</p>"
            );

            return Ok("Correo enviado correctamente.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al enviar el correo: {ex.Message}");
        }
    }
}


