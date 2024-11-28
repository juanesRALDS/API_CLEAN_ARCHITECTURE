using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace api_completa_mongodb_net_6_0.Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginUserUseCase _loginUserUseCase;

    public AuthController(LoginUserUseCase loginUserUseCase)
    {
        _loginUserUseCase = loginUserUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        try
        {
            string? token = await _loginUserUseCase.ExecuteAsync(loginDto);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Credenciales inválidas.");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto userDto)
    {
        try
        {
            await _loginUserUseCase.RegisterAsync(userDto);
            return Ok("Usuario registrado exitosamente.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

