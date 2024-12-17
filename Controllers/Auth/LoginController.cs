using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;
using Microsoft.AspNetCore.Mvc;

namespace api_completa_mongodb_net_6_0.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class LoginController : ControllerBase
{
    private readonly ILoginUseCase _loginUserUseCase;


    public LoginController(ILoginUseCase loginUserUseCase)
    {
        _loginUserUseCase = loginUserUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        try
        {
            string? tokens = await _loginUserUseCase.Execute(loginDto);

            return Ok(new { Tokens = tokens });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Credenciales inválidas.");
        }
    }
}

