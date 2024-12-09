using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using Microsoft.AspNetCore.Mvc;
using MongoApiDemo.Domain.Interfaces.Auth.IAuthUsecases;

namespace api_completa_mongodb_net_6_0.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class LoginController : ControllerBase
{
    private readonly ILoginUseCase _loginUserUseCase;
    private readonly IRegisterUseCase _registerUseCase;

    public LoginController(ILoginUseCase loginUserUseCase, IRegisterUseCase registerUseCase)
    {
        _loginUserUseCase = loginUserUseCase;
        _registerUseCase = registerUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        try
        {
            string? tokens = await _loginUserUseCase.Login(loginDto);

            string callbackUrl = $"{Request.Scheme}://{Request.Host}/api/auth/reset-password?token={tokens}";

            Console.WriteLine(callbackUrl);

            return Ok(new { Tokens = tokens });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Credenciales inválidas.");
        }
    }
}

